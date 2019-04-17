string Exec()
{
    if(IsRunningOnWindows())
    {
        return "./rancher/rancher-compose.exe";
    }

    return "./rancher/rancher-compose";
}

var environment = Argument("Env", "preprod");
var tag = Argument("tag","latest");

class RancherConfig
{
    public string ServerUrl {get;set;}
    public string AccessKey {get;set;}
    public string SecretKey {get;set;}
    public string DefaultStack {get;set;}
}

var settings = DeserializeJsonFromFile<RancherConfig>($"environments/{environment}.rancher.json");

Task("Deploy") // Publishing backend artifacts for CI (backend tests included)...
    .Does(() =>
{
    using(var process = StartAndReturnProcess(Exec(), new ProcessSettings
    {
        Arguments = $"--project-name {settings.DefaultStack} --file docker-compose.{environment}.yml --rancher-file rancher-compose.{environment}.yml up -c -u -d --batch-size 1 --pull",
        EnvironmentVariables = new Dictionary<string,string>
        {
            {"RANCHER_URL", settings.ServerUrl},
            {"RANCHER_ACCESS_KEY", settings.AccessKey},
            {"RANCHER_SECRET_KEY", Decrypt(settings.SecretKey)},
            {"tag", tag}
        },
        WorkingDirectory = "environments"
    }))
    {
        process.WaitForExit();
        var code = process.GetExitCode();
        Information("Exit code: {0}", code);

        if(code != 0) {
            throw new Exception("Rancher process failed.");
        }
    }
});