#addin nuget:?package=Cake.FileHelpers&version=3.1.0

void PublishContainerLogs(string publishDir)
{
    IEnumerable<string> redirectedStandardOutput;
    IEnumerable<string> redirectedError;
    var exitCode = StartProcess(
         "docker",
         new ProcessSettings {
             Arguments = "logs rest_scaffold",
             RedirectStandardOutput = true,
             RedirectStandardError= true
         },
         out redirectedStandardOutput,
         out redirectedError
     );
    var logContent = new string[] 
    {  
        string.Format("Standard output:\r\n{0}", string.Join("\r\n", redirectedStandardOutput)),
        string.Format("Standard error:\r\n{0}", string.Join("\r\n", redirectedError))
    };
    FileWriteLines("../publish/Lni.log", logContent);
}

void PublishMetrics(string publishDir) 
{
    DownloadFile("http://localhost:8060/metrics", "../publish/Lni.metrics");
    DownloadFile("http://localhost:8060/metrics-text", "../publish/Lni.metrics.txt");
}

void PublishEnv(string publishDir)
{
    DownloadFile("http://localhost:8060/env", "../publish/Lni.env");
}

void PublishSwagger(string publishDir)
{
    DownloadFile("http://localhost:8060/swagger/v1/swagger.json", "../publish/swagger.json");
} 