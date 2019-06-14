#addin nuget:?package=Cake.Docker&version=0.9.6

#addin nuget:?package=Cake.Json
#addin nuget:?package=Newtonsoft.Json&version=9.0.1

var imageTag = Argument("image_tag", "latest");
var composeFile = string.Format("./environments/docker-compose.{0}.yml", Argument<string>("Env", "prod"));
var envComposeFile = string.Format("./environments/docker-compose.{0}.yml", "dependencies");

Task("Package") // Publishing backend artifacts for CI (backend tests included)...
    .Does(() =>
{
    Console.Out.WriteLine("##teamcity[buildNumber '"+ imageTag + "']");

    foreach(var dockerFile in System.IO.Directory.GetFiles(publishDir, "Dockerfile",System.IO.SearchOption.AllDirectories))
    {
		var dockerFilePath = System.IO.Path.GetDirectoryName(dockerFile);
		var projectName = ((DirectoryPath)(MakeAbsolute(Directory(dockerFilePath)).FullPath)).GetDirectoryName().ToLower();
        BuildDockerImage(imageTag, dockerFilePath, projectName);
    }
});

Task("SetupDependencies") // Build and run a Docker integration environment stack
    .Does(() =>
{
    Console.WriteLine($"Running compose up with compose file {envComposeFile}");

    DockerComposeUp(new DockerComposeUpSettings
    {
        Files = new [] {envComposeFile},
        Build = true,
        ForceRecreate = true,
        DetachedMode = true,
        RemoveOrphans = false
    });
});

Task("SetupDevEnvironment") // Build and run a Docker integration environment stack
    .Does(() =>
{
    Console.WriteLine($"Running compose up with compose file {composeFile}");

    DockerComposeUp(new DockerComposeUpSettings
    {
        Files = new [] {composeFile},
        Build = true,
        ForceRecreate = true,
        DetachedMode = true,
        RemoveOrphans = false
    });
});

Task("CleanDependencies") // Cleaning the existing Docker integration environment...
    .Does(() =>
{
    Console.WriteLine($"Running compose down with compose file {envComposeFile}");

    DockerComposeDown(new DockerComposeDownSettings
    {
        Files = new [] {envComposeFile},
        RemoveOrphans = false,
        Volumes = true,
        Rmi = "local"
    });    
});

Task("CleanDevEnvironment") // Cleaning the existing Docker integration environment...
    .Does(() =>
{
    Console.WriteLine($"Running compose down with compose file {composeFile}");

    DockerComposeDown(new DockerComposeDownSettings
    {
        Files = new [] {composeFile},
        RemoveOrphans = false,
        Volumes = true,
        Rmi = "local"
    });    
});

void BuildDockerImage(string imageTag, string binDir, string projectName)
{
    Information($"Building docker image for {projectName}");
    DockerBuild(new DockerImageBuildSettings
    {
        Tag = new string[] { GetFullDockerPath(imageTag, projectName) },
        BuildArg = new string[] { "TARGET_VERSION=Release" }
    },
    binDir);
}

string GetFullDockerPath(string tag, string projectName)
{
    return "pocket_monsters-" + projectName + ":" + imageTag;
}

/********* ALIASES **********/

Task("Setupd")
    .IsDependentOn("SetupDevEnvironment");

Task("Cleand")
    .IsDependentOn("CleanDevEnvironment");

Task("Setupdp")
    .IsDependentOn("SetupDependencies");

Task("Cleandp")
    .IsDependentOn("CleanDependencies");