FROM mcr.microsoft.com/dotnet/sdk:5.0
WORKDIR /app
EXPOSE 5000
ENV ASPNETCORE_URLS=http://0.0.0.0:5000
COPY build/ ./
ENTRYPOINT ["dotnet", "ZenAchitecture.WebUI.dll"]