#addin nuget:?package=Cake.Coverlet&version=2.2.1
#addin nuget:?package=Cake.FileHelpers

#load "docker.cake"

var target = Argument("target", "Default");
var configuration = Argument("configuration", "Release");
var frontConfiguration = Argument("front", "developement");
var version = Argument("productversion", "1.0.0");
var location = EnvironmentVariable("CAKE_ENVIRONMENT") ?? "desktop";

var sln = "../PocketMonsters.sln";
var publishDir = "../publish/";

FilePath npmPath = Context.Tools.Resolve("npm.cmd");

var dotnetProjects = new [] { "Accounts", "Monsters", "Trading" };
var mavenProjects = new [] { "gateway", "eureka", "ressources", "news"};

Task("FrontClean")
    .Does(() =>
    {
        CleanDirectory("../src/gateway/src/main/resources/static");
        CleanDirectory("../src/front/dist/front");
    });

Task("Clean")
    .Does(() =>
    {
        CleanDirectory(publishDir);
        CleanDirectories(string.Format("../**/obj/{0}", configuration));
        CleanDirectories(string.Format("../**/bin/{0}", configuration));
    });

Task("DotnetBuild") // Building Lni.sln...
    .IsDependentOn("Clean")
    .Does(() =>
    {
        DotNetCoreBuild(sln, new DotNetCoreBuildSettings
        {
            Configuration = configuration,
            ArgumentCustomization = args => args.Append("/p:Version=" + version)
        });
    });

Task("Test") // Running unit tests...
    .IsDependentOn("DotnetBuild")
    .Does(() =>
    {
        foreach(var project in GetFiles("../test/**/*.Tests.csproj"))
        {
            var testSettings = new DotNetCoreTestSettings
            {
                Configuration = configuration,
                NoBuild = true,
                ArgumentCustomization = args => args.Append("--no-restore --filter \"Category!=Integration\" --verbosity normal")
            };

            TestWithCoverage(project, testSettings);
        }
    });


Task("RunIntegrationTests") // Running integration tests from previously published artifacts  (using ASP.NET Core integration settings)...
    .Does(() => Environment.SetEnvironmentVariable("TEST_ENVIRONMENT", "integration"))
    .DoesForEach(GetFiles("../test/**/*.Tests.csproj"),
        (project) =>
        {
            var dummyWaitForIt = 18000;
            Information($"Wait {dummyWaitForIt/1000} seconds for backends to be up ... (! we cannot live with that !)");
            System.Threading.Thread.Sleep(dummyWaitForIt);
            var testSettings = new DotNetCoreTestSettings
            {
                Configuration = configuration,
                NoBuild = false,
                ArgumentCustomization = args => args.Append("--filter \"Category=Integration\" --verbosity normal")
            };

            TestWithCoverage(project, testSettings);
        }
    );

Task("Integration") // Build an integration environment and run integration tests from previously published tests artifacts.
    .IsDependentOn("CleanDevEnvironment")
    .IsDependentOn("SetupDevEnvironment")
    .IsDependentOn("RunIntegrationTests");

Task("Publish") // Publishing backend artifacts for CI (backend tests included)...
    .IsDependentOn("Test")
    .Does(() =>
{
    foreach (var project in dotnetProjects)
    {
        var outputDirectory = publishDir + project;
		Information($"Publishing {project} ({outputDirectory})");

        DotNetCorePublish("../src/" + project, new DotNetCorePublishSettings
        {
            Configuration = configuration,
            OutputDirectory = outputDirectory,
            ArgumentCustomization = args => args.Append("--no-restore /p:Version=" + version)
        });
		CopyFile($"../src/{project}/Dockerfile", System.IO.Path.Combine(publishDir, $"{project}/Dockerfile"));
    }
	
	// Publishing all backend tests
	foreach (var project in GetFiles("../test/**/*.csproj"))
    {
        DotNetCorePublish(project.FullPath, new DotNetCorePublishSettings
        {
            Configuration = configuration,
            OutputDirectory = publishDir + project.GetFilenameWithoutExtension().FullPath,
            ArgumentCustomization = args => args.Append("--no-restore /p:Version=" + version)
        });
    }
});

Task("DockerBuild")
    .Does(() =>
    {
        foreach(var dockerFile in System.IO.Directory.GetFiles(publishDir, "Dockerfile",System.IO.SearchOption.AllDirectories))
        {
            var dockerFilePath = System.IO.Path.GetDirectoryName(dockerFile);
            var projectName = ((DirectoryPath)(MakeAbsolute(Directory(dockerFilePath)).FullPath)).GetDirectoryName().ToLower();
            BuildDockerImage(imageTag, dockerFilePath, projectName);
            // DockerPushImage(imageTag, projectName);
        }
    });

Task("MavenBuild").Does(() =>
{
    Console.WriteLine("");
    Console.WriteLine($"Cake environment is {location}");
    Console.WriteLine("");

    foreach (var project in mavenProjects)
    {
        Console.WriteLine("");
        Console.WriteLine("Building maven project {0}", project);
        Console.WriteLine("");

        if (location == "desktop")
        {
        	ExecuteProcess("D:/Programs/maven/bin/mvn.cmd", "-f", $"../src/{project}", "compile", "com.google.cloud.tools:jib-maven-plugin:1.1.1:dockerBuild");
        }
        else
        {
        	ExecuteProcess("C:/Program Files/maven/bin/mvn.cmd", "-f", $"../src/{project}", "compile", "com.google.cloud.tools:jib-maven-plugin:1.1.1:dockerBuild");
        }
    }
});

Task("BuildFront")
    .IsDependentOn("FrontClean")
    .Does(() =>
    {
        ExecuteProcess(npmPath.FullPath, "run", "prod-build", "--prefix", "../src/front");
        CopyDirectory("../src/front/dist/front", "../src/gateway/src/main/resources/static");
    });

Task("Default") // Build and run unit tests only, then publish the application (frontend + backend)
    .IsDependentOn("BuildFront")
    .IsDependentOn("DotnetBuild")
    .IsDependentOn("Test")
    .IsDependentOn("Publish")
    .IsDependentOn("DockerBuild")
    .IsDependentOn("MavenBuild");

RunTarget(target);

/*
 * Execute an arbitrary process
 * Used to call Maven as there are no official support for it
 */
public void ExecuteProcess(string process, params string[] args)
{
    var processArgumentBuilder = new ProcessArgumentBuilder();

    foreach (var arg in args)
    {
        processArgumentBuilder.Append(arg);
    }

    StartProcess(process, new ProcessSettings {
            Arguments = processArgumentBuilder
        }
    );
}

/*
 * Generate Coverlet results
 */
void TestWithCoverage(FilePath project, DotNetCoreTestSettings testSettings)
{
    var coverageResults = "../coverage-results";
    var coverletOutputDirectory = Directory(coverageResults);

    var coverletSettings = new CoverletSettings {
        CollectCoverage = true,
        CoverletOutputFormat = CoverletOutputFormat.opencover,
        CoverletOutputDirectory = coverletOutputDirectory,
        CoverletOutputName = $"results-{DateTime.UtcNow:dd-MM-yyyy-HH-mm-ss-FFF}"
    };

    DotNetCoreTest(project.FullPath, testSettings, coverletSettings);
    
    var coverletOutputFile = System.IO.Path.Combine(MakeAbsolute(coverletOutputDirectory).FullPath, "*.opencover.xml");
    var reportOutputDirectory = Directory(coverletOutputDirectory.Path + "/report");
    Information("Launching report from {0} in folder {1}", coverletOutputFile, reportOutputDirectory);

    ReportGenerator(coverletOutputFile, reportOutputDirectory, new ReportGeneratorSettings {
        ReportTypes = new ReportGeneratorReportType[]{ReportGeneratorReportType.Badges, ReportGeneratorReportType.Html},
        AssemblyFilters = new String[]{"-xunit*"}
    });

    Console.WriteLine($"##teamcity[publishArtifacts 'coverage-results']");
}

void BuildFront()
{

}