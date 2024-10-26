# UnityToggleTitleBar
A simple editor script that maximizes Unity's window area by hiding title bar and menu (Windows only)

![UnityToggleTitleBar in action](https://rostok.github.io/cdn/UnityToggleTitleBar/screen-cap-960.gif)

# Installation
Since Unity doesn't support GIT url dependencies in package.json installation requires adding first extender package and then this package. To do so paste into Unity's package manager (PLUS SIGN, add package from GIT URL) the following urls:
- first https://github.com/marijnz/unity-toolbar-extender.git
- then https://github.com/rostok/UnityToggleTitleBar.git

Alternatively, you can just edit Packages/manifes.json file in your project and add these two lines to dependencies section:
```
    "com.marijnzwemmer.unity-toolbar-extender": "https://github.com/marijnz/unity-toolbar-extender.git",
    "com.github.rostok.unitytoggletitlebar": "https://github.com/rostok/UnityToggleTitleBar.git",
```


# Usage
Press F11 or button left of Undo History to toggle the title bar and menu.

# Dependencies
https://github.com/marijnz/unity-toolbar-extender

# License
MIT with additional condition that you can't use this for anything concerning military.


