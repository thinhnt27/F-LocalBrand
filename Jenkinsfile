pipeline {

    agent any

    
    stages {

        stage('Packaging') {

            steps {
                
                sh 'docker build --pull --rm -f Dockerfile -t flocalbrandapi:latest .'
                
            }
        }

        stage('Push to DockerHub') {

            steps {
                withDockerRegistry(credentialsId: 'dockerhub', url: 'https://index.docker.io/v1/') {
                    sh 'docker tag flocalbrandapi:latest chalsfptu/flocalbrandapi:latest'
                    sh 'docker push chalsfptu/flocalbrandapi:latest'
                }
            }
        }

        stage('Deploy FE to DEV') {
            steps {
                echo 'Deploying and cleaning'
                sh 'docker container stop flocalbrandapi || echo "this container does not exist" '
                sh 'echo y | docker system prune '
                sh 'docker container run -d --rm --name flocalbrandapi -p 8082:8080 -p 8083:8081  chalsfptu/flocalbrandapi '
            }
        }
        
 
    }
    post {
        always {
            cleanWs()
        }
    }
}