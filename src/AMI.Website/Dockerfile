FROM microsoft/dotnet:2.2-aspnetcore-runtime AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM microsoft/dotnet:2.2-sdk AS build
WORKDIR /src
COPY ["AMI.Website/AMI.Website.csproj", "AMI.Website/"]
RUN dotnet restore "AMI.Website/AMI.Website.csproj"
COPY . .
WORKDIR "/src/AMI.Website"
RUN dotnet build "AMI.Website.csproj" -c Release -o /app

FROM build AS publish
RUN dotnet publish "AMI.Website.csproj" -c Release -o /app

FROM base AS final
WORKDIR /app
COPY --from=publish /app .
ENTRYPOINT ["dotnet", "AMI.Website.dll"]