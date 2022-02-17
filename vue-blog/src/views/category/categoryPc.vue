<template>
  <div
    :style="{
      background: `url(${bgImg}) 0px center no-repeat`,
      backgroundSize: 'cover',
    }"
    class="common"
  >
    <div class="d3Chart"></div>
  </div>
</template>
<script>
import * as d3 from 'd3';
import { GetCategoryTree } from 'api';
export default {
  name: 'CategoryPc',
  data() {
    return {
      bgImg: '/images/category.png'
    };
  },
  mounted() {
    this.getCategoryTree();
  },
  methods: {
    getCategoryTree() {
      this.$progress.start();
      GetCategoryTree().then(res => {
        this.$progress.done();
        if (res.statusCode < 1) {
          return this.$toast.error(res.msg);
        }
        let data = { label: '博文分类', id: '', children: res.data };
        this.loadTree(data);
      });
    },
    loadTree(data) {
      let that = this;
      // 画布的参数
      let mapWidth = 650;
      let mapHeight = 600;
      let mapPadding = 20;
      // 定义画布—— 外边距 10px
      let svgMap = d3.select('.d3Chart').append('svg').attr('width', mapWidth).attr('height', mapHeight).style('margin', '10px');
      // 定义树状图画布
      let treeMap = svgMap.append('g').attr('transform', 'translate(' + mapPadding + ',' + mapPadding + ')');
      // 将源数据转换为可以生成树状图的数据(有节点 nodes 和连线 links )
      let treeData = d3.tree()
        // 设置树状图的尺寸
        .size([mapWidth - 3 * mapPadding, mapHeight - 6 * mapPadding])
        // 设置树状图节点之间的间隔
        .separation(function(a, b) {
          return (a.parent == b.parent ? 1 : 2) / a.depth;
        })(
          // 创建层级布局，对源数据进行数据转换
          d3.hierarchy(data).sum(function(d) {
            return d.value;
          })
        );
      // 贝塞尔曲线生成器
      let Bézier_curve_generator = d3.linkHorizontal()
        .x(function(d) {
          return d.y;
        })
        .y(function(d) {
          return d.x;
        });
      // 绘制边
      treeMap.selectAll('path')
        // 节点的关系 links
        .data(treeData.links())
        .enter()
        .append('path')
        .attr('d', function(d) {
          // 根据name值的长度调整连线的起点
          var start = { x: d.source.x, y: d.source.y + d.source.data.label.length * 16 + 12 };
          var end = { x: d.target.x, y: d.target.y };
          return Bézier_curve_generator({ source: start, target: end });
        })
        .attr('fill', 'none')
        .attr('stroke', '#00e676')
        .attr('stroke-width', 1);
      // 创建分组——节点+文字
      let groups = treeMap.selectAll('g')
        // 节点 nodes
        .data(treeData.descendants())
        .enter()
        .append('g')
        .attr('transform', function(d) {
          var cx = d.x;
          var cy = d.y;
          return 'translate(' + cy + ',' + cx + ')';
        })
        .on('mouseover', function(d, node) {
          d3.select(this).append('circle')
            .attr('r', 5)
            .attr('fill', 'white')
            .attr('stroke', 'red')
            .attr('stroke-width', 2);
        })
        .on('mouseout', function(d, node) {
          d3.select(this).append('circle')
            .attr('r', 5)
            .attr('fill', 'white')
            .attr('stroke', '#00e676')
            .attr('stroke-width', 2);
        });
      // 绘制节点
      groups.append('circle')
        .attr('r', 5)
        .attr('fill', 'white')
        .attr('stroke', '#00e676')
        .attr('stroke-width', 2);

      // 绘制文字
      groups.append('text')
        .attr('x', function(d) {
          return 8;
        })
        .attr('y', -5)
        .attr('dy', 10)
        .text(function(d) {
          return d.data.label;
        })
        .style('fill', 'rgb(247 253 255)')
        .style('font-weight', 'bold')
        .style('font-size', '0.36rem')
        .style('cursor', 'pointer')
        .style('transition', '0.6s ease-in-out')
        .on('click', function($event, e) {
          // d3.event.sourceEvent.stopPropagation();
          // d3.event.sourceEvent.preventDefault();
          // console.log('this', this);
          // console.log($event, e); // 显示所点击节点数据
          let queryData = e.data.id ? { categoryid: e.data.id } : null;
          that.$router.push({ name: 'articles', query: queryData });
        }).on('mouseover', function(d, node) {
          d3.select(this).style('font-size', '0.43rem');
        }).on('mouseout', function(d, node) {
          d3.select(this).style('font-size', '0.36rem');
        });
    }
  }

};
</script>
<style scoped>
  .d3Chart {
    text-align: center;
    position: absolute;
    top: 64px !important;
    bottom: 0;
    overflow: auto;
    width: 100%;
    padding-top: 20px;
    color: #fff;
    margin: 0;
  }
</style>
