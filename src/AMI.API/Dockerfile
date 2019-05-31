FROM microsoft/dotnet:2.2-aspnetcore-runtime AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM microsoft/dotnet:2.2-sdk AS build
WORKDIR /src
COPY . .
RUN dotnet restore "AMI.API/AMI.API.csproj"
WORKDIR "/src/AMI.API"
RUN dotnet build "AMI.API.csproj" -c Release -o /app

FROM build AS publish
RUN dotnet publish "AMI.API.csproj" -c Release -o /app

FROM base AS final
WORKDIR /app
COPY --from=publish /app .
ENTRYPOINT ["dotnet", "AMI.API.dll"]