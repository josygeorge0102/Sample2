import jenkins.model.*
jenkins = Jenkins.instance
pipeline {
     agent any
     
       
    
    stages {

        stage ('Checkout') {
             steps{
                   checkout([$class: 'GitSCM', branches: [[name: '*/master']], extensions: [], userRemoteConfigs: [[credentialsId: '1685bc3f-c82f-441d-a812-0f287757f7c8', url: 'https://github.com/josygeorge0102/Sample2.git']]])
             }        
        }
    
        
       
    // Pushing Docker images into ACR
    stage('Pushing Docker Images to ACR') {
     steps{   
         withCredentials([usernamePassword(credentialsId: 'ACR', passwordVariable: 'ACR_PASSWORD', usernameVariable: 'ACR_USER')]){
            bat 'docker login oesregistry.azurecr.io -u $ACR_USER -p $ACR_PASSWORD'
            bat'docker-compose push'
          }
       }
      }
    }
}

      
      
   
   
  
   
   
  
