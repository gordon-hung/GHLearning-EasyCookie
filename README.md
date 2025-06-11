# GHLearning-EasyCookie
[![.NET](https://github.com/gordon-hung/GHLearning-EasyCookie/actions/workflows/dotnet.yml/badge.svg)](https://github.com/gordon-hung/GHLearning-EasyCookie/actions/workflows/dotnet.yml) [![Ask DeepWiki](https://deepwiki.com/badge.svg)](https://deepwiki.com/gordon-hung/GHLearning-EasyCookie)  [![codecov](https://codecov.io/gh/gordon-hung/GHLearning-EasyCookie/graph/badge.svg?token=C8QLMXHVIO)](https://codecov.io/gh/gordon-hung/GHLearning-EasyCookie)

## 目的和範圍
本文檔全面概述了 GHLearning.EasyCookie 系統，這是一個 .NET 9 Web API 項目，旨在學習現代雲端原生開發實務。該系統演示了基於 Cookie 的身份驗證、分散式追蹤、監控和簡潔的架構模式。

本概述涵蓋了系統的核心目的、架構基礎和關鍵功能。有關詳細的架構分析，請參閱系統架構。有關身份驗證系統的實作細節，請參閱身份驗證和帳戶管理。有關部署和品質實踐，請參閱CI/CD 和品質保證。

## 系統概述
GHLearning.EasyCookie 是一個可立即投入生產的 Web API，它實現了基於 Cookie 的身份驗證，並支援 Redis 會話管理。該系統既是現代 .NET 開發實踐的學習平台，也是雲端原生模式（包括 OpenTelemetry 可觀測性、健康檢查和自動化測試）的參考實作。

本系統主要透過無狀態 Cookie 驗證實現帳戶管理，並輔以展現典型 API 模式的天氣預報端點。所有操作均配備全面的監控和分散式追蹤功能。

## 入門

該系統旨在實現即時本地開發和部署。關鍵入口點包括：

- API 探索/swagger：端點可用的 Swagger UI
- 健康監測：即時狀態/live和詳細健康狀況/healthz
- 身份驗證流程：帳戶登入/登出端點演示基於 cookie 的身份驗證
- 可觀察性：內建 OpenTelemetry 追蹤和 Prometheus 指標

有關環境設定的詳細信息，請參閱開發環境。有關監控和可觀察性配置的詳細信息，請參閱監控和可觀察性。有關全面的測試方法，請參閱測試策略。
