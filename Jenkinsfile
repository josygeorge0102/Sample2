node{

 
  stage('SCM Checkout'){
    git 'https://github.com/josygeorge0102/OnlineEducationSystem.git'
  }
 
  
  stage('Docker compose push'){
     script{
        bat 'docker login oesregistry.azurecr.io -u OESRegistry -p EWdqcMQEOfw8NbUGFm7=3eO5=U94c5M4'
        bat 'docker-compose push'
     }
   } 
      
}
  
   
   
  
