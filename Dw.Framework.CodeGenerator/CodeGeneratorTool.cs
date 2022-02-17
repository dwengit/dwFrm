using Dw.Framework.CodeGenerator.Model;
using JinianNet.JNTemplate;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Dw.Framework.CodeGenerator
{
    public class CodeGeneratorTool
    {
        /// <summary>
        /// 生成代码
        /// </summary>
        /// <param name="dto"></param>
        public static void Generate(GenerateDto dto)
        {
            IList<CodeGenerateConfig> generateConfigs = CodeGenerateConfig.BuildDefault(dto.NamespaceName, dto.TableColumns);
            foreach (var config in generateConfigs)
            {
                dto.GenCodeDto = config;
                InitJntTemplate(dto);
                GenerateModels(dto, config);
                GenerateDto(dto, config);
                GenerateIRepository(dto, config);
                GenerateRepository(dto, config);
                GenerateIAppService(dto, config);
                GenerateAppService(dto, config);
                GenerateMap(dto, config);
                GenerateController(dto, config);
            }
            foreach (var item in dto.GenCodes)
            {
                FileHelper.WriteAndSave(item.Path, item.Content);
            }
        }

        /// <summary>
        /// 生成实体类Model
        /// </summary>
        /// <param name="generateDto"></param>
        /// <param name="replaceDto">替换实体</param>
        private static void GenerateModels(GenerateDto generateDto, CodeGenerateConfig config)
        {
            var tpl = FileHelper.ReadJtTemplate("Tpl_Model.txt");
            var result = tpl.Render();

            string fullPath = Path.Combine(generateDto.GenCodePath, config.ModelsNamespace, "Models", config.CodeName + ".cs");
            generateDto.GenCodes.Add(new GenCode("Model.cs", fullPath, result));
        }
        /// <summary>
        /// 生成Dto层代码文件
        /// </summary>
        /// <param name="generateDto"></param>
        /// <param name="replaceDto">替换实体</param>
        private static void GenerateDto(GenerateDto generateDto, CodeGenerateConfig config)
        {
            var tpl = FileHelper.ReadJtTemplate("Tpl_Dto.txt");
            var result = tpl.Render();
            string fileName = $"{config.CodeName}Dto.cs";
            string fullPath = Path.Combine(generateDto.GenCodePath, config.ModelsNamespace, "Dto", fileName);
            generateDto.GenCodes.Add(new GenCode(fileName, fullPath, result));
        }
        /// <summary>
        /// 生成IRepository层代码文件
        /// </summary>
        /// <param name="generateDto"></param>
        /// <param name="replaceDto">替换实体</param>
        private static void GenerateIRepository(GenerateDto generateDto, CodeGenerateConfig config)
        {
            var tpl = FileHelper.ReadJtTemplate("Tpl_IRepositorys.txt");
            var result = tpl.Render();
            string fileName = $"I{config.CodeName}Repository.cs";
            string fullPath = Path.Combine(generateDto.GenCodePath, config.BaseNamespace + "ApplicationCore", "Repository", config.BusTypeName, fileName);
            generateDto.GenCodes.Add(new GenCode(fileName, fullPath, result));
        }
        /// <summary>
        /// 生成Repository层代码文件
        /// </summary>
        /// <param name="generateDto"></param>
        /// <param name="replaceDto">替换实体</param>
        private static void GenerateRepository(GenerateDto generateDto, CodeGenerateConfig config)
        {
            var tpl = FileHelper.ReadJtTemplate("Tpl_Repositorys.txt");
            var result = tpl.Render();
            string fileName = $"{config.CodeName}Repository.cs";
            string fullPath = Path.Combine(generateDto.GenCodePath, config.BaseNamespace + "ApplicationCore", "Repository", config.BusTypeName, fileName);
            generateDto.GenCodes.Add(new GenCode(fileName, fullPath, result));
        }
        /// <summary>
        /// 生成IAppService层代码文件
        /// </summary>
        /// <param name="generateDto"></param>
        /// <param name="replaceDto">替换实体</param>
        private static void GenerateIAppService(GenerateDto generateDto, CodeGenerateConfig config)
        {
            var tpl = FileHelper.ReadJtTemplate("Tpl_IAppService.txt");
            var result = tpl.Render();
            string fileName = $"{config.CodeName}IAppService.cs";
            string fullPath = Path.Combine(generateDto.GenCodePath, config.BaseNamespace + "ApplicationCore", "AppServices", config.BusTypeName, "IService", fileName);
            generateDto.GenCodes.Add(new GenCode(fileName, fullPath, result));
        }
        /// <summary>
        /// 生成AppService层代码文件
        /// </summary>
        /// <param name="generateDto"></param>
        /// <param name="replaceDto">替换实体</param>
        private static void GenerateAppService(GenerateDto generateDto, CodeGenerateConfig config)
        {
            var tpl = FileHelper.ReadJtTemplate("Tpl_AppService.txt");
            var result = tpl.Render();
            string fileName = $"{config.CodeName}AppService.cs";
            string fullPath = Path.Combine(generateDto.GenCodePath, config.BaseNamespace + "ApplicationCore", "AppServices", config.BusTypeName, fileName);
            generateDto.GenCodes.Add(new GenCode(fileName, fullPath, result));
        }
        /// <summary>
        /// 生成Map层代码文件
        /// </summary>
        /// <param name="generateDto"></param>
        /// <param name="replaceDto">替换实体</param>
        private static void GenerateMap(GenerateDto generateDto, CodeGenerateConfig config)
        {
            var tpl = FileHelper.ReadJtTemplate("Tpl_Map.txt");
            var result = tpl.Render();
            string fileName = $"{config.BusTypeName}Map.cs";
            string fullPath = Path.Combine(generateDto.GenCodePath, config.BaseNamespace + "ApplicationCore", "AutoMapperProfile", fileName);
            generateDto.GenCodes.Add(new GenCode(fileName, fullPath, result));
        }
        /// <summary>
        /// 生成Controller层代码文件
        /// </summary>
        /// <param name="generateDto"></param>
        /// <param name="replaceDto">替换实体</param>
        private static void GenerateController(GenerateDto generateDto, CodeGenerateConfig config)
        {
            var tpl = FileHelper.ReadJtTemplate("Tpl_Controller.txt");
            var result = tpl.Render();
            string fileName = $"{config.AbbrCodeName}Controller.cs";
            string fullPath = Path.Combine(generateDto.GenCodePath, config.ApiControllerNamespace, "Controllers", "v1", config.BusTypeName, fileName);
            generateDto.GenCodes.Add(new GenCode(fileName, fullPath, result));
        }

        /// <summary>
        /// 初始化Jnt模板
        /// </summary>
        /// <param name="dto"></param>
        /// <param name="replaceDto"></param>
        private static void InitJntTemplate(GenerateDto dto)
        {
            //jnt模板引擎全局变量
            Engine.Configure((options) =>
            {
                options.TagPrefix = "${";
                options.TagSuffix = "}";
                options.TagFlag = '$';
                options.OutMode = OutMode.Auto;
                options.Data.Set("genCodeDto", dto.GenCodeDto);
                options.Data.Set("replaceDto", dto);
                options.EnableCache = true;
            });
        }

    }
}
