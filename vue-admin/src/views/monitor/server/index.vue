<template>
  <div class="app-container">
    <el-row>
      <el-col :lg="24" class="card-box">
        <el-card class="box-card">
          <div slot="header" class="clearfix">
            <span style="font-weight: bold;color: #666;font-size: 15px">状态</span>
          </div>
          <div>
            <el-col :xs="24" :sm="24" :md="6" :lg="6" :xl="6" style="margin-bottom: 10px">
              <div class="title">CPU使用率</div>
              <el-tooltip placement="top-end">
                <div v-if="server.cpu" class="content">
                  <el-progress type="dashboard" :percentage="parseFloat(server.cpu.cpuRate)" />
                </div>
              </el-tooltip>
              <div v-if="server.sys" class="footer">{{ server.sys.cpuNum }} 核心</div>
            </el-col>
            <el-col v-if="server.cpu" :xs="24" :sm="24" :md="6" :lg="6" :xl="6" style="margin-bottom: 10px">
              <div class="title">内存使用率</div>
              <el-tooltip placement="top-end">
                <div slot="content" style="font-size: 12px;">
                  <div style="padding: 3px;">
                    总量：{{ server.cpu.totalRAM }}
                  </div>
                  <div style="padding: 3px">
                    已使用：{{ server.cpu.usedRam }}
                  </div>
                  <div style="padding: 3px">
                    空闲：{{ server.cpu.freeRam }}
                  </div>
                </div>
                <div class="content">
                  <el-progress type="dashboard" :percentage="parseFloat( server.cpu.ramRate)" />
                </div>
              </el-tooltip>
              <div class="footer"> {{ server.cpu.usedRam }} / {{ server.cpu.totalRAM }}</div>
            </el-col>

            <el-col v-for="sysFile in server.disk" :key="sysFile.diskName" :xs="24" :sm="24" :md="6" :lg="6" :xl="6" style="margin-bottom: 10px">
              <div class="title">{{ sysFile.diskName }}盘使用率</div>
              <div class="content">
                <el-tooltip placement="top-end">
                  <div slot="content" style="font-size: 12px;">
                    <div style="padding: 3px">
                      总量：{{ sysFile.totalSize }}GB
                    </div>
                    <div style="padding: 3px">
                      空闲：{{ sysFile.availableFreeSpace }}GB
                    </div>
                    <div style="padding: 3px">
                      已用：{{ sysFile.used }}GB
                    </div>
                  </div>
                  <div class="content">
                    <el-progress type="dashboard" :percentage="parseFloat(sysFile.availablePercent)" />
                  </div>
                </el-tooltip>
              </div>
              <div class="footer">{{ sysFile.availableFreeSpace }}GB可用，共{{ sysFile.totalSize }}GB</div>
            </el-col>
          </div>
        </el-card>
      </el-col>

      <el-col :lg="24" class="card-box">
        <el-card>
          <div slot="header">
            <span>服务器信息</span>
          </div>
          <div class="el-table el-table--enable-row-hover el-table--medium">
            <table v-if="server.sys" cellspacing="0" style="width: 100%;">
              <tbody>
                <tr>
                  <td>
                    <div class="cell">服务器名称</div>
                  </td>
                  <td>
                    <div class="cell">{{ server.sys.computerName }}</div>
                  </td>
                  <td>
                    <div class="cell">操作系统</div>
                  </td>
                  <td>
                    <div class="cell">{{ server.sys.osName }}</div>
                  </td>
                </tr>
                <tr>
                  <td>
                    <div class="cell">服务器IP</div>
                  </td>
                  <td>
                    <div class="cell">{{ server.sys.serverIP }}</div>
                  </td>
                  <td>
                    <div class="cell">系统架构</div>
                  </td>
                  <td>
                    <div class="cell">{{ server.sys.osArch }}</div>
                  </td>
                </tr>
                <tr>
                  <td>
                    <div class="cell">系统运行时长</div>
                  </td>
                  <td>
                    <div class="cell">{{ server.sys.runTime }}</div>
                  </td>
                  <td>
                    <div class="cell" />
                  </td>
                  <td>
                    <div class="cell" />
                  </td>
                </tr>
              </tbody>
            </table>
          </div>
        </el-card>
      </el-col>

      <!-- <el-col :span="24" class="card-box">
        <el-card>
          <div slot="header">
            <span>磁盘状态</span>
          </div>
          <el-col v-for="sysFile in server.disk" :key="sysFile.diskName" :xs="24" :sm="24" :md="6" :lg="6" :xl="6" style="margin-bottom: 10px">
            <div class="title">{{ sysFile.diskName }}盘使用率</div>
            <div class="content">
              <el-tooltip placement="top-end">
                <div slot="content" style="font-size: 12px;">
                  <div style="padding: 3px">
                    总量：{{ sysFile.totalSize }}GB
                  </div>
                  <div style="padding: 3px">
                    空闲：{{ sysFile.availableFreeSpace }}GB
                  </div>
                  <div style="padding: 3px">
                    已用：{{ sysFile.used }}GB
                  </div>
                </div>
                <div class="content">
                  <el-progress type="dashboard" :percentage="parseFloat(sysFile.availablePercent)" />
                </div>
              </el-tooltip>
            </div>
            <div class="footer">{{ sysFile.availableFreeSpace }}GB可用，共{{ sysFile.totalSize }}GB</div>
          </el-col>
        </el-card>
      </el-col> -->

      <el-col :lg="24" class="card-box">
        <el-card>
          <div slot="header">
            <span>.NET Core信息</span>
          </div>
          <div class="el-table el-table--enable-row-hover el-table--medium">
            <table cellspacing="0" style="width: 100%;">
              <tbody>
                <tr>
                  <td>
                    <div class="cell">环境变量</div>
                  </td>
                  <td>
                    <div v-if="server.app" class="cell">{{ server.app.name }}</div>
                  </td>
                  <td>
                    <div class="cell">.Net版本</div>
                  </td>
                  <td>
                    <div v-if="server.app" class="cell">{{ server.app.version }}</div>
                  </td>
                </tr>
                <tr>
                  <td>
                    <div class="cell">启动时间</div>
                  </td>
                  <td>
                    <div v-if="server.app" class="cell">{{ server.app.startTime }}</div>
                  </td>
                  <td>
                    <div class="cell">运行时长</div>
                  </td>
                  <td>
                    <div v-if="server.app" class="cell">{{ server.app.runTime }}</div>
                  </td>
                </tr>
                <tr>
                  <td>
                    <div class="cell">占用内存</div>
                  </td>
                  <td>
                    <div v-if="server.app" class="cell">{{ server.app.appRAM }}</div>
                  </td>
                  <td>
                    <div class="cell">启动地址</div>
                  </td>
                  <td>
                    <div v-if="server.app" class="cell">{{ server.app.host }}</div>
                  </td>
                </tr>
                <tr>
                  <td>
                    <div class="cell">ContentRootPath</div>
                  </td>
                  <td>
                    <div v-if="server.app" class="cell">{{ server.app.rootPath }}</div>
                  </td>
                  <td>
                    <div class="cell">webPath</div>
                  </td>
                  <td>
                    <div v-if="server.app" class="cell">{{ server.app.webRootPath }}</div>
                  </td>
                </tr>
              </tbody>
            </table>
          </div>
        </el-card>
      </el-col>

    </el-row>
  </div>
</template>

<script>
import { GetServerInfo } from '@/api/monitor';

export default {
  name: 'Server',
  data() {
    return {
      // 服务器信息
      server: [],
      intervalId: null
    };
  },
  created() {
    this.getList();
    this.dataRefreh();
  },
  destroyed() {
    this.clear();
  },
  methods: {
    /** 查询服务器信息 */
    getList() {
      GetServerInfo(this.queryForm).then(res => {
        if (!res.success) {
          this.$message.error(res.msg);
          return;
        }
        this.server = res.data;
      }).catch(err => {
        this.$message.error(err.msg);
      });
    },
    dataRefreh() {
      if (this.intervalId != null) {
        return;
      }
      this.intervalId = setInterval(() => {
        this.getList();
      }, 10000);
    },
    /**
     * 停止定时器
     */
    clear() {
      clearInterval(this.intervalId);
      this.intervalId = null;
    }
  }
};
</script>
<style scoped>
table tr {
  height: 30px;
}
.is-leaf {
  text-align: center;
  padding: 0 10px;
}
.title {
  text-align: center;
  font-size: 15px;
  font-weight: 500;
  color: #999;
  margin-bottom: 16px;
}
.footer {
  text-align: center;
  font-size: 15px;
  font-weight: 500;
  color: #999;
  margin-top: -5px;
  margin-bottom: 10px;
}
.content {
  text-align: center;
  margin-top: 5px;
  margin-bottom: 5px;
}
</style>
