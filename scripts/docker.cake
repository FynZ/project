#addin nuget:?package=Cake.Docker&version=0.9.6

#addin nuget:?package=Cake.Json
#addin nuget:?package=Newtonsoft.Json&version=9.0.1

class DockerConfig
{
    public string RegistryServer {get;set;}
    public string DefaultImageTag {get;set;}
    public string User {get;set;}
    public string Password {get;set;}
    public string RegistryPath {get;set;}
}

var imageTag = Argument("image_tag", "latest");

string GetFullDockerPath(string tag, string projectName)
{
    return "pocket_monsters-" + projectName + ":" + imageTag;
}

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

var envComposeFile = string.Format("./environments/docker-compose.{0}.yml", Argument<string>("Env", "prod"));

Task("CleanDevEnvironment") // Cleaning the existing Docker integration environment...
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

Task("SetupDevEnvironment") // Build and run a Docker integration environment stack
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