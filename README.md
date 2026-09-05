# scoredp_desktop

beatmania IIDX DP 서열표 기록용 Windows 데스크톱 앱

## 기능

- **사용자** - 기록할 사용자 등록, 관리
- **기록** - 곡별 정보 입력 및 확인
- **서열표** - 곡별 서열 확인
- **랜덤** - 난이도 구간을 지정해 무작위 곡 선택
- **서열표 동기화** - [zasa 비공식 난이도표](https://zasa.sakura.ne.jp/dp/)에서 최신 정보를 가져와 갱신

## 기술

- .NET 10 (WPF + Blazor Hybrid, Microsoft.AspNetCore.Components.WebView.Wpf)
- WebView2를 통한 Razor 컴포넌트 렌더링 with Tailwind CSS
- SQLite (EF Core) - `App_Data/scoredp.db`
- HtmlAgilityPack - zasa 서열표 페이지 파싱

## 개발 환경

1. [WebView2 Runtime](https://developer.microsoft.com/microsoft-edge/webview2/) 필요합니다. (Windows 11은 기본 포함)
2. Tailwind CSS는 빌드마다 `tools/tailwindcss.exe`(standalone CLI)로 재컴파일됩니다. [Tailwind 릴리즈 페이지](https://github.com/tailwindlabs/tailwindcss/releases)에서 Windows용 `tailwindcss.exe`를 받아 `ScoreDp.Desktop/tools/tailwindcss.exe`에 저장해주세요.

## 빌드 & 실행

```bash
dotnet build ScoreDp.Desktop
dotnet run --project ScoreDp.Desktop
```

## 배포용 빌드

```bash
dotnet publish ScoreDp.Desktop -c Release -r win-x64 --self-contained -p:PublishSingleFile=true -o publish
```

`wwwroot/`, `App_Data/`는 exe와 같은 폴더에 있어야 합니다.
