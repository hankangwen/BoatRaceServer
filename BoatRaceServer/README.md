> 本文由 [简悦 SimpRead](http://ksria.com/simpread/) 转码， 原文地址 [blog.csdn.net](https://blog.csdn.net/guonanjun/article/details/40985041)

### 文件系统得是 NTFS，而且是 Vista 及以上才行。

MKLINK [[/D] | [/H] | [/J]] Link Target

/D 创建目录符号链接。默认为文件  
符号链接。  
/H 创建硬链接，而不是符号链接。  
/J 创建目录联接。  
Link 指定新的符号链接名称。  
Target 指定新链接引用的路径  
(相对或绝对)。

MKLINK 这个命令不但可以创建 unix 系统那样的符号链接、硬链接，还可以创建从 win2000 就有的目录联接 (junction point)。  
如果执行这个命令时系统提示 “ **您没有足够的权限执行此操作。**”  
可以在 win7 开始菜单的 “搜索程序和文件” 框中输入 cmd，然后按住 shift，右键 cmd， 选择“以管理员身份运行”，得到一个有足够权限执行 mklink 的命令行窗口。

删除指向文件的符号链接，用 del  
删除指向文件夹的符号链接，用 rmdir

用法举例：  
mklink b.txt a.txt  
创建符号链接 b.txt，指向 a.txt

mklink/d dyn-el ..\epm\dyn-el  
创建符号链接 dyn-el，指向上级目录下的 epm 目录下的子目录 dyn-el


在 Vista 以下如 xp 是没有 mklink 这个命令的，就只能创建传统的目录联接 (junction point) 了。  
这需要用到一个名为 Junction 的命令行工具，可以在 http://technet.microsoft.com/en-us/sysinternals/bb896768 下载。

用法举例：  
junction b a  
建立一个目录联接 b，指向目录 a

junction -d b  
删除目录联接 b