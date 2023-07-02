<h1>Create 3 microservices, push to ECR and Run in ECS FARGATE</h1>
<p>
<h2> Build the images </h2>
<ul>
  <li>docker build -f Dockerfile-Report . </li>
  <li>docker build -t microservice-report -f Dockerfile-Report . (build with a name) </li>
</ul>

</p>


<h2> Create a user (with ECR permissions) and download the access key id and secret </h2>
<ul>
  <li>access_key_id : ************ </li>
  <li>aws_secret_access_key: ********* </li>
</ul>

</p>
<p>
  <h2> Create a profile for the user in the AWS Console </h2>
<ul>
  <li>aws configure --profile ecr-agent (login to AWS CLi with that profile </li>
</ul>
</p>
<p>
<h2> login to ECR and push to the repositories </h2>
<ul>
  <li>aws ecr get-login-password --region eu-north-1 --profile ecr-agent | docker login --username AWS --password-stdin {7digitId}.dkr.ecr.eu-north-1.amazonaws.com/precipitation </li>
</ul>

</p>
<p>
<h2> Tag the images </h2>
<ul>
  <li> docker tag microservicesprecipitation:dev {7digitId}.dkr.ecr.eu-north-1.amazonaws.com/precipitation:1</li>
  <li> docker tag microservice-report {7digitId}.dkr.ecr.eu-north-1.amazonaws.com/report:1 </li>
  <li> docker tag microservicetemperature:dev {7digitId}.dkr.ecr.eu-north-1.amazonaws.com/temperature:1</li>
</ul>
</p>
<p>
  <h2> push images to ecr </h2>
<ul>
  <li> docker push {7digitId}.dkr.ecr.eu-north-1.amazonaws.com/precipitation:1</li>
  <li> docker push {7digitId}.dkr.ecr.eu-north-1.amazonaws.com/temperature:1 </li>
  <li> docker push {7digitId}.dkr.ecr.eu-north-1.amazonaws.com/report:1</li>
</ul>
</p>
<p>
   <h2>Generate sql scripts from migration (this will generate the ef migrations history table also) </h2>
<ul>
  <li> dotnet ef migrations script --idempotent -o initial-migration </li>
</ul>
</p>
