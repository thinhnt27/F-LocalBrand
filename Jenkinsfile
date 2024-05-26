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
                withCredentials([file(credentialsId: 'be', variable: 'FILE'),
                [
                    SECRET_KEY: 'SECRET_KEY',
                    DB_SERVER: 'DB_SERVER',
                    DB_NAME: 'DB_NAME',
                    DB_USER: 'DB_USER',
                    DB_PASSWORD: 'DB_PASSWORD',
                    DB_TRUST_SERVER_CERTIFICATE: 'DB_TRUST_SERVER_CERTIFICATE',
                    DB_MULTIPLE_ACTIVE_RESULT_SETS: 'DB_MULTIPLE_ACTIVE_RESULT_SETS'
                ]])
                {
                    echo 'Deploying and cleaning'
                    sh 'docker container stop flocalbrandapi || echo "this container does not exist" '
                    sh 'echo y | docker system prune '
                    sh '''docker container run -e SECRET_KEY="${SECRET_KEY}" -e DB_SERVER="${DB_SERVER}" -e DB_NAME="${DB_NAME}" -e DB_USER="${DB_USER}" -e DB_PASSWORD="${DB_PASSWORD}" -e DB_TRUST_SERVER_CERTIFICATE="${DB_TRUST_SERVER_CERTIFICATE}" -e DB_MULTIPLE_ACTIVE_RESULT_SETS="${DB_MULTIPLE_ACTIVE_RESULT_SETS}" -d --rm --name flocalbrandapi -p 8082:8080 -p 8083:8081 chalsfptu/flocalbrandapi '''
                }
            }
        }
        
 
    }
    post {
        always {
            cleanWs()
        }
    }
}