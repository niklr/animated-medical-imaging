FROM microsoft/dotnet:2.2-aspnetcore-runtime AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM microsoft/dotnet:2.2-sdk AS build
WORKDIR /src
COPY ["AMI.Website/AMI.Website.csproj", "AMI.Website/"]
RUN dotnet restore "AMI.Website/AMI.Website.csproj"
COPY . .
COPY ["RnsCodeAnalysis.ruleset", "SharedAssemblyInfo.cs", "AMI.Website/"]
WORKDIR "/src/AMI.Website"

# Source: https://stackoverflow.com/questions/51918919/how-to-integrate-npm-install-into-asp-net-core-2-1-docker-build
# Install curl, git, nano
RUN apt-get update -yq && apt-get upgrade -yq && apt-get install -yq curl git nano
# Install nodejs, build-essential
RUN curl -sL https://deb.nodesource.com/setup_8.x | bash - && apt-get install -yq nodejs build-essential
RUN npm install -g npm
RUN npm install

RUN dotnet build "AMI.Website.csproj" -c Release -o /app

FROM build AS publish
RUN dotnet publish "AMI.Website.csproj" -c Release -o /app

FROM base AS final
WORKDIR /app
COPY --from=publish /app .
ENTRYPOINT ["dotnet", "AMI.Website.dll"]