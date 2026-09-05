window.scoredp = window.scoredp || {};

// 캡처본은 7열 그리드 강제
window.scoredp.captureScoreGrid = async function (elementId, filename) {
    const el = document.getElementById(elementId);
    if (!el) return;

    const clone = el.cloneNode(true);
    clone.querySelectorAll(".song-grid").forEach((g) => {
        g.className = "song-grid grid grid-cols-7 gap-1.5";
    });

    const hider = document.createElement("div");
    hider.style.cssText = "position:fixed;left:-10000px;top:0;";
    const wrapper = document.createElement("div");
    wrapper.style.cssText = "width:1200px;padding:16px;box-sizing:border-box;background:#0f0f1a;";
    wrapper.appendChild(clone);
    hider.appendChild(wrapper);
    document.body.appendChild(hider);

    await new Promise((r) => requestAnimationFrame(() => requestAnimationFrame(r)));

    try {
        const url = await htmlToImage.toPng(wrapper, { backgroundColor: "#0f0f1a", pixelRatio: 4 });
        const a = document.createElement("a");
        a.href = url;
        a.download = filename;
        a.click();
    } finally {
        document.body.removeChild(hider);
    }
};
