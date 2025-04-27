
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

https://medium.com/@tom-010/tcr-variants-test-commit-revert-bf6bd84b17d3
```TCR
dotnet build && ( dotnet test && git commit -am "r" || git reset --hard)

```