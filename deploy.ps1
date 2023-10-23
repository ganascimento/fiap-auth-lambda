dotnet restore ./AuthLambda
dotnet lambda package --project-location ./AuthLambda --output-package auth_lambda.zip --configuration Release --framework net6.0
aws lambda update-function-code --function-name AuthLambda --zip-file fileb://./auth_lambda.zip