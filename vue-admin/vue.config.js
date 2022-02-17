const path = require('path');

function resolve(dir) {
  return path.join(__dirname, dir);
}
module.exports = {
  configureWebpack: {
    resolve: {
      alias: {
        '@': resolve('src'),
        'components': resolve('src/components'),
        'api': resolve('src/api'),
        'utils': resolve('src/utils'),
        'store': resolve('src/store'),
        'router': resolve('src/router')
      }
    }
  },
  chainWebpack(config) {
    config.module
      .rule('svg')
      .exclude.add(resolve('src/icons'))
      .end();
    config.module
      .rule('icons')
      .test(/\.svg$/)
      .include.add(resolve('src/icons'))
      .end()
      .use('svg-sprite-loader')
      .loader('svg-sprite-loader')
      .options({
        symbolId: 'icon-[name]'
      })
      .end();
  },
  publicPath: process.env.VUE_APP_ROUTER_BASE_DIR || '/', // 部署后的相对路径
  outputDir: './dist_admin', // 打包的目录
  lintOnSave: true, // 在保存时校验格式
  productionSourceMap: false, // 生产环境是否生成 SourceMap
  devServer: {
    open: true, // 启动服务后是否打开浏览器
    host: '0.0.0.0',
    port: 8080, // 服务端口
    https: false,
    hotOnly: false,
    proxy: {
      '/api': { // 匹配规则，映射请求/api代理到指定服务器
        target: process.env.VUE_APP_API_HOST, // 代理的服务器url
        ws: false, // 是否启用websockets代理
        changeOrigin: true, // 把代理服务器url作为代理
        pathRewrite: {
          '^/api': process.env.VUE_APP_API_VERSION // 把/api替换为指定路径
        }
      }
    }
  }
};
