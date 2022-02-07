
pipeline{
  agent any  
  stages {

        stage ('checkout') {
            steps {
                git 'https://github.com/josygeorge0102/Sample2.git'
            }
        }
       stage ('Docker Compose Push') {
            steps {               
                
                    bat 'docker login oesregistry.azurecr.io -u OESRegistry -p EWdqcMQEOfw8NbUGFm7=3eO5=U94c5M4'
                    bat'docker-compose push'
                  
            }
        }
      
  }
}
