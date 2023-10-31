dockerPipeline {
  helmRepository = 'https://github.com/Monster-OCS/ocs-helm-charts.git'
  helmDirectory = 'micm-apps'
  projectType = 'dotnet6'
  //we can't deploy to other environment then DEV yet
  skipDeployTo = ['ams-qax5', 'qax7', 'ams-qax1', 'ams-prod']
}
