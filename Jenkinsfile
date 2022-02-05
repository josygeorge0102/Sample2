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
     withCredentials([usernamePassword(credentialsId: 'ACR', passwordVariable: 'ACR_PASSWORD', usernameVariable: 'ACR_USER')]){
        bat 'docker login oesregistry.azurecr.io -u $ACR_USER -p $ACR_PASSWORD'
        bat'docker-compose push'
      }
     }
      
}
  
   
   
  
