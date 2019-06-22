FROM microsoft/dotnet:2.2-aspnetcore-runtime AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM microsoft/dotnet:2.2-sdk AS build
WORKDIR /src
COPY . .
RUN dotnet restore "AMI.API/AMI.API.csproj"
WORKDIR /src/libs
ADD https://github.com/niklr/SimpleITK/releases/download/v1.2.0/SimpleITK-1.2.0-CSharp-linux_x86_64.tar.gz .
RUN tar -xzf SimpleITK-1.2.0-CSharp-linux_x86_64.tar.gz --strip-components=1 \
 && rm SimpleITK-1.2.0-CSharp-linux_x86_64.tar.gz \
 && cd .. \
 && mkdir -p /libs \
 && mv /src/libs/* /libs
WORKDIR /src/AMI.API
RUN dotnet build "AMI.API.csproj" -c Release -r linux-x64 -o /app

FROM build AS publish
RUN dotnet publish "AMI.API.csproj" -c Release -r linux-x64 -o /app

FROM base AS final
WORKDIR /app
COPY --from=publish /app .
COPY --from=build /libs/libSimpleITKCSharpNative.so .
ENV LD_LIBRARY_PATH="/app:${LD_LIBRARY_PATH}"
# install System.Drawing native dependencies
RUN apt-get update \
    && apt-get install -y --allow-unauthenticated \
        libc6-dev \
        libgdiplus \
        libx11-dev \
     && rm -rf /var/lib/apt/lists/*
# udpate lib cache
RUN ldconfig
ENTRYPOINT ["dotnet", "AMI.API.dll"]