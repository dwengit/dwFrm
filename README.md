# Dw.Frm V1.0

#### 在线体验
##### 后台管理  
https://admin.yousu.xyz/admin/login  
```
账号：admin    
密码：123456
```
##### 前台博客  
https://blog.yousu.xyz/home
```
由于资金有限，（缓存服务、对象存储服务、持久化存储服务）的服务器离web服务器很远，速度会有点慢。
```
#### 介绍
Dw.Frm一个轻量简单的项目，层次结构简单清晰，很适合新手上手也很容进行扩展和二次开发，本项目前后端分离模式，开箱即用，只需要关注业务功能开发。  
Dw.Frm内置权限管理和博客管理模块，权限认证使用Jwt，实现的通用权限管理RBAC模式。  
由于历史原因使用的.net core3.1，后期架构将全面重构为NET6，敬请期待。

#### 相关技术
##### 前端技术
使用Vue-cil进行框架的搭建，使用到vue、vuex、axios、vue-router、mavon-editor、markdown-it、element-ui、must-ui、jquery、d3js、echarts

##### 后端技术
基于.NetCore3.1开发，使用到EntityFrameworkCore、SqlSugar、log4net、Minio、AutoMapper、Dapper、JinianNet.JNTemplate

##### 数据存储
对象存储(minio)，持久化存储（mysql），缓存（redis）

##### 部署环境
centos7.6+docker

#### 项目结构
![image](https://github.com/dwengit/dwFrm/blob/main/READMEIMG/frmdic.png)

#### 演示图
![image](https://github.com/dwengit/dwFrm/blob/main/READMEIMG/login.png)
![image](https://github.com/dwengit/dwFrm/blob/main/READMEIMG/home.png)
![image](https://github.com/dwengit/dwFrm/blob/main/READMEIMG/sys.png)
![image](https://github.com/dwengit/dwFrm/blob/main/READMEIMG/blog.png)
![image](https://github.com/dwengit/dwFrm/blob/main/READMEIMG/tools.png)
![image](https://github.com/dwengit/dwFrm/blob/main/READMEIMG/mt.png)
![image](https://github.com/dwengit/dwFrm/blob/main/READMEIMG/bloghome.png)
![image](https://github.com/dwengit/dwFrm/blob/main/READMEIMG/blogct.png)
![image](https://github.com/dwengit/dwFrm/blob/main/READMEIMG/blogat.png)


###### 参考与借鉴(panjiachen.github.io,nevergiveupt.top/index,gitee.com/izory/ZrAdminNetCore)

