import axios from 'utils/http-axios.js';

// 获取所有的数据名称
export const GetAllDataBases = (data) => axios.get('/CodeGenerator/GetAllDataBases', {
  params: { ...data }
});
// 获取所有表根据数据库名
export const GetAllTables = (data) => axios.get('/CodeGenerator/GetAllTables', {
  params: { ...data }
});
// 生成打包代码
export const CodeGenerate = (data) => axios.post('/CodeGenerator/CodeGenerate', {
  ...data
});
