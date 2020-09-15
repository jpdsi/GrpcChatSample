# GrpcChatSample

本リポジトリは、ブログ記事「[gRPC で動く CLI チャットアプリを ASP.NET Core で実装してみよう！](https://jpdsi.github.io/blog/web-apps/GrpcChatOnAspNetCore/)」内で紹介した gRPC で動く CLI チャットアプリのサンプルリポジトリです。

## 構成
本リポジトリの構成は以下の通りです。


```
GrpcChatSample/
├─ GrpcChatServer/
├─ GrpcChatClient/
└─ GrpcChatClientGo/
```


- `GrpcChatServer`：C# で書かれたチャットサーバ
- `GrpcChatClient`：C# で書かれたチャットクライアント
- `GrpcChatClientGo`：Go で書かれたチャットクライアント

## 検証済み環境

本リポジトリのコードは下記環境にて検証しました。

- OS：Windows 10 Enterprise バージョン 1903
- IDE：Visual Studio 2019
- PlatForm：.NET Core 3.1.401
- protoc.exe：libprotoc 3.13.0
- go：version go1.15 windows/amd64

## 実行方法

### `GrpcChatServer` について

`GrpcChatSample/GrpcChatServer/GrpcChatServer.sln` を Visual Studio 2019 から開き、実行する。

もしくは `GrpcChatSample/GrpcChatServer/GrpcChatServer` まで移動して

```shell
$ dotnet run
```

### `GrpcChatClient` について

`GrpcChatSample/GrpcChatClient/GrpcChatClient.sln` を Visual Studio 2019 から開き、実行する。

もしくは `GrpcChatSample/GrpcChatClient/GrpcChatClient` まで移動して

```shell
$ dotnet run
```

### `GrpcChatClientGo` について

`GrpcChatSample/GrpcChatClientGo/chat/client` まで移動して

```shell
$ go run client.go
```

---
なお、本サンプルは弊社の公式サンプルではなく、今後の動作を保証するものではありません。また、go 言語のサンプルアプリのサポートやそれに関する環境構築に関するサポートは弊社では行っておりませんことご了承ください。