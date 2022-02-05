node{

  agent any
  environment{
      registryName="OESRegistry"
      registryUrl="oesregistry.azurecr.io"
      registryCredential="ACR"
   }
  stage('SCM Checkout'){
    git 'https://github.com/josygeorge0102/Sample2.git'
  }
 
  
  stage('Docker compose push'){
     script{
      docker.withRegistry("http://${registryUrl}",registryCredential){
        bat 'docker-compose push'
      }
     }
   } 
      
}
  
   
   
  
