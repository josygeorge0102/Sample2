node{
    stage('SCM Checkout'){
        git 'https://github.com/josygeorge0102/Sample.git'
    }
    stage('Build Docker Image'){
       sh 'docker build -t josy98/classroomserviceapi:v1 .'
    }
}