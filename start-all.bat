@echo off
chcp 65001 >nul
cd /d "%~dp0"
title RepairIT - Lancement

echo ==========================================
echo   RepairIT - Lancement de tous les projets
echo ==========================================
echo.
echo Compilation de la solution...
dotnet build RepairIT.sln --nologo -v q
if errorlevel 1 (
    echo.
    echo Nouvel essai apres nettoyage des fichiers intermediaires du client...
    if exist "src\RepairIT.Client\obj" rmdir /s /q "src\RepairIT.Client\obj"
    dotnet build RepairIT.sln --nologo -v q
    if errorlevel 1 (
        echo.
        echo La compilation a echoue. Corrigez les erreurs puis relancez ce script.
        pause
        exit /b 1
    )
)

echo.
echo Demarrage des microservices...
start "RepairIT.CatalogService - 5101" /D "%~dp0src\RepairIT.CatalogService" dotnet run --no-build --launch-profile http
start "RepairIT.UserService - 5102" /D "%~dp0src\RepairIT.UserService" dotnet run --no-build --launch-profile http
start "RepairIT.RepairService - 5103" /D "%~dp0src\RepairIT.RepairService" dotnet run --no-build --launch-profile http
start "RepairIT.SalesService - 5104" /D "%~dp0src\RepairIT.SalesService" dotnet run --no-build --launch-profile http
start "RepairIT.AuthentificationService - 5105" /D "%~dp0src\RepairIT.AuthentificationService" dotnet run --no-build --launch-profile http
start "RepairIT.Gateway - 5100" /D "%~dp0src\RepairIT.Gateway" dotnet run --no-build --launch-profile http
start "RepairIT.Client - 5000" /D "%~dp0src\RepairIT.Client" dotnet run --no-build --launch-profile http

echo.
echo Services lances :
echo   Client Blazor          http://localhost:5000
echo   Administration         http://localhost:5000/admin
echo   Gateway                http://localhost:5100
echo   CatalogService         http://localhost:5101/swagger
echo   UserService            http://localhost:5102/swagger
echo   RepairService          http://localhost:5103/swagger
echo   SalesService           http://localhost:5104/swagger
echo   AuthentificationService http://localhost:5105/swagger
echo.
echo Fermez chaque fenetre (ou Ctrl+C) pour arreter un service.
