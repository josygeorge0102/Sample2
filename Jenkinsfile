import jenkins.model.*
jenkins = Jenkins.instance
pipeline {
     agent any
     
       
    
    stages {

        stage ('SCM Checkout') {
            steps {
            checkout([$class: 'GitSCM', branches: [[name: '*/master']], doGenerateSubmoduleConfigurations: false, extensions: [], submoduleCfg: [], userRemoteConfigs: [[url: 'https://github.com/josygeorge0102/Sample2.git']]])
            }
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

      
      
   
   
  
   
   
  
