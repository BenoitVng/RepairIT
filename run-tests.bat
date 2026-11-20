@echo off
chcp 65001 >nul
cd /d "%~dp0"
set CONFIG=DataCollectionRunSettings.DataCollectors.DataCollector.Configuration

if exist TestResults rmdir /s /q TestResults

dotnet test RepairIT.sln --collect:"XPlat Code Coverage" --results-directory TestResults -- ^
    "%CONFIG%.Format=cobertura" ^
    "%CONFIG%.Exclude=[*.Tests]*" ^
    "%CONFIG%.ExcludeByFile=**/Migrations/*.cs,**/Program.cs" ^
    "%CONFIG%.ExcludeByAttribute=GeneratedCodeAttribute,CompilerGeneratedAttribute,ExcludeFromCodeCoverageAttribute"

echo.
echo Rapports de couverture (Cobertura) generes dans le dossier TestResults.
pause
