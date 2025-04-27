
``` bash
dotnet tool restore
dotnet restore
dotnet build  --no-restore --configuration Release -p:AnalysisLevel=latest /warnaserror
dotnet test /p:AltCover=true /p:AltCoverLocalSource=true --no-restore
reportgenerator -reports:"**/coverage.xml" -targetdir:Coverage
open Coverage/index.html
```


``` bash
dotnet tool restore
dotnet stryker -o
```