## 🧰 环境与安装
### 1. 安装 Unity
- 使用 **Unity Hub**
- 版本号：**2022.3.34f1c1**
- 平台：macOS 或 Windows 都可  
  - 如果两人系统不同，版本号保持一致即可（Unity 项目是跨平台的）

### 2. 克隆项目
```bash
git clone https://github.com/Zihang-He/Lost-Ones-2D-Game.git
cd Lost-Ones-2D-Game
git lfs install
git lfs pull


Assets/
├─ Scripts/ # 游戏逻辑脚本 (ActRunner.cs 等)
├─ Data/ # 剧情文件，例如 story.json
├─ Prefabs/ # 可复用的 UI 元件，例如按钮预制体
├─ Resources/
│ └─ Art/ # 背景图资源 (lake.png, cabin.png ...)
└─ Scenes/ # 主场景 (Main.unity)