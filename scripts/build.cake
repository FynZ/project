#tool "nuget:?package=ReportGenerator"
#addin nuget:?package=Cake.Coverlet
#addin nuget:?package=Cake.FileHelpers

var target = Argument("target", "Default");
var configuration = Argument("configuration", "Release");
var version = Argument("productversion", "1.0.0");

var sln = "../PocketMonsters.sln";
var publishDir = "../publish/";

#load "docker.cake"
#load "exporters.cake"
#load "rancher.cake"

var publishedProjects = new [] { "Accounts", "Monsters" };

Setup(ctx =>
{
    Information($"Building Lni {version}");
});

Task("Clean").Does(() =>
{
    CleanDirectory(publishDir);
    CleanDirectories(string.Format("../**/obj/{0}", configuration));
    CleanDirectories(string.Format("../**/bin/{0}", configuration));
});

Task("Build") // Building Lni.sln...
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
    .IsDependentOn("Build")
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
    // .DeferOnError()
    // .Finally(() =>
    // {
        // //Environment.SetEnvironmentVariable("TEST_ENVIRONMENT", null);
        // //PublishSwagger(publishDir);
        // //PublishContainerLogs(publishDir);
        // //PublishMetrics(publishDir);
        // //PublishEnv(publishDir);
    // });

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

Task("Integration") // Build an integration environment and run integration tests from previously published tests artifacts.
    .IsDependentOn("CleanDevEnvironment")
    .IsDependentOn("SetupDevEnvironment")
    .IsDependentOn("RunIntegrationTests");

Task("Publish") // Publishing backend artifacts for CI (backend tests included)...
    .IsDependentOn("Test")
    .Does(() =>
{

    foreach (var project in publishedProjects)
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

    {
        // Publishing sql
        // var outputDirectory = publishDir+"lni-sql";
        // var project = "sql";
        // Information($"Publishing {project} ({outputDirectory})");
        // CreateDirectory(outputDirectory);
        // CopyDirectory("../src/" + project, outputDirectory);
    }
	
    Console.WriteLine("##teamcity[publishArtifacts 'publish']");

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

Task("Default") // Build and run unit tests only, then publish the application (frontend + backend)
    .IsDependentOn("Build")
    .IsDependentOn("Test")
    .IsDependentOn("Publish")
    .IsDependentOn("DockerBuild");

void ReplaceInFileContent(string file, string old, string target)
{
    var text =System.IO.File.ReadAllText(file);
    var diff = text.Replace(old, target);
    if(text !=diff)
        System.IO.File.WriteAllText(file, diff);
}

void ReplaceText(string old, string target)
{
    var files = GetFiles("../**/*", x => !x.Path.FullPath.Contains(".git") && !x.Path.FullPath.Contains("tools"));

    foreach(var f in files.OrderByDescending(d=>d.FullPath.Length))
    {
        ReplaceInFileContent(f.FullPath, old, target);
    }


    Console.WriteLine("Moving dirs");
    var dirNames = System.IO.Directory.GetDirectories("..", $"*{old}*" , System.IO.SearchOption.AllDirectories);

    foreach(var dir in dirNames)
    {
        if(!System.IO.Directory.Exists(dir))
            continue;

        var after = dir.Replace(old, target);
        Console.WriteLine($"{dir} => {after}");
        System.IO.Directory.Move(dir , after);
    }


    Console.WriteLine("Moving files");
    var fileNames = System.IO.Directory.GetFiles("..", $"*{old}*" , System.IO.SearchOption.AllDirectories);

    foreach(var f in fileNames)
    {
        var after = f.Replace(old, target);
        Console.WriteLine($"{f} => {after}");
        System.IO.File.Move(f, after);
    }
}

string[] GetDifferentForms(string text)
{
    var tokens = text.Split('-');

    return new []
    {
        string.Join("", tokens.Select(t=>t.ToUpper()[0] + t.Substring(1))),
        string.Join(" ", tokens.Select(t=>t.ToUpper()[0] + t.Substring(1))),
        string.Join("-", tokens),
        string.Join("_", tokens),
        string.Join(" ", tokens),
    };
}

Task("configure") // Publishing backend artifacts for CI (backend tests included)...
    .Does(() =>
{
    var name = Argument<string>("name");

    var oldNames = GetDifferentForms("lni");
    var newNames = GetDifferentForms(name);

    for(int i=0;i<oldNames.Length;i++)
    {
        Console.WriteLine($"{oldNames[i]} => {newNames[i]}");
        ReplaceText(oldNames[i], newNames[i]);
    }
});

Task("DockerBuild").Does(() =>
{
    foreach(var dockerFile in System.IO.Directory.GetFiles(publishDir, "Dockerfile",System.IO.SearchOption.AllDirectories))
    {
		var dockerFilePath = System.IO.Path.GetDirectoryName(dockerFile);
		var projectName = ((DirectoryPath)(MakeAbsolute(Directory(dockerFilePath)).FullPath)).GetDirectoryName().ToLower();
        BuildDockerImage(imageTag, dockerFilePath, projectName);
        // DockerPushImage(imageTag, projectName);
    }
});

RunTarget(target);