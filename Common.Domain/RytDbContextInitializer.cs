using Common.Util;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.Domain
{
    public class RytDbContextInitializer : DropCreateDatabaseIfModelChanges<CommonContext>
    {
        protected override void Seed(CommonContext db)
        {
            var IntegralGradeData = db.Set<IntegralGrade>().AddRange(new List<IntegralGrade>()
            {
                new IntegralGrade{  Grade=1,  Title="1"},
                new IntegralGrade{  Grade=2,  Title="2"},
                new IntegralGrade{  Grade=3,  Title="3"},
            });

            var HospitalData=db.Set<Hospital>().AddRange(new List<Hospital>()
            {
                new Hospital{ Name="北京协和医院", FullName="北京协和医院", Level=9 },
                new Hospital{ Name="北京地坛医院", FullName="北京地坛医院", Level=9 },
                new Hospital{ Name="宣武医院", FullName="首都医科大学宣武医院", Level=9 },
            });

            var MedicineCategoryData=db.Set<MedicineCategory>().AddRange(new List<MedicineCategory>()
            {
                new MedicineCategory{ Name="肿瘤内科", Order=1},
                new MedicineCategory{ Name="肿瘤外科", Order=2},
                new MedicineCategory{ Name="放疗科", Order=3},
            });

            foreach (var h in HospitalData)
            {
                foreach (var c in MedicineCategoryData)
                {
                    db.Set<MedicineDepartment>().Add(new MedicineDepartment
                    {
                        Hospital = h,
                        MedicineCategory = c,
                        Doctors = new List<Doctor>()
                        {
                            new Doctor{  Name=ChineseHelper.GetRandomChineseName(), Title="主治医师", Expert="肺癌、乳腺癌，胃癌的化疗，靶向治疗，内分泌治疗，免疫治疗，联合中医药治疗，癌痛治疗", Academic="复旦大学肿瘤学专业，医学博士，副主任医师", Introduction="，男，38岁，复旦大学肿瘤学专业，医学博士，副主任医师。从事肿瘤内科临床工作15年，熟悉常见肿瘤的诊治原则，擅长结直肠癌、胃癌和肺癌的系统性化疗和靶向治疗。        主要从事肿瘤分子靶向治疗和耐药领域的基础、临床和转化性研究，参与国家“十一五重大攻关计划分课题：分子靶向药物评价平台的建设”、“十二五科技重大专项：国际标准抗肿瘤新药临床试验平台建设”等重大专项科研项目，并主持国家自然科学基金面上项目、上海市自然科学基金项目。先后在国内外发表研究论文20余篇，代表性研究成果发表于“Clinical Cancer Research”，“BMC Medicine”，“Oncotarget”等国际知名学术期刊。        2015年参与完成“胃癌发生发展的分子机制研究与临床治疗新策略的建立和应用”项目，荣获中华人民共和国教育部“科学技术进步奖二等奖”。        2016年参加上海市卫计委组织的“援滇扶贫”任务，荣获复旦大学附属肿瘤医院“对口支援工作杰出贡献奖”。" },
                            new Doctor{  Name=ChineseHelper.GetRandomChineseName(), Title="主治医师", Expert="肺癌、乳腺癌，胃癌的化疗，靶向治疗，内分泌治疗，免疫治疗，联合中医药治疗，癌痛治疗", Academic="复旦大学肿瘤学专业，医学博士，副主任医师", Introduction="，男，38岁，复旦大学肿瘤学专业，医学博士，副主任医师。从事肿瘤内科临床工作15年，熟悉常见肿瘤的诊治原则，擅长结直肠癌、胃癌和肺癌的系统性化疗和靶向治疗。        主要从事肿瘤分子靶向治疗和耐药领域的基础、临床和转化性研究，参与国家“十一五重大攻关计划分课题：分子靶向药物评价平台的建设”、“十二五科技重大专项：国际标准抗肿瘤新药临床试验平台建设”等重大专项科研项目，并主持国家自然科学基金面上项目、上海市自然科学基金项目。先后在国内外发表研究论文20余篇，代表性研究成果发表于“Clinical Cancer Research”，“BMC Medicine”，“Oncotarget”等国际知名学术期刊。        2015年参与完成“胃癌发生发展的分子机制研究与临床治疗新策略的建立和应用”项目，荣获中华人民共和国教育部“科学技术进步奖二等奖”。        2016年参加上海市卫计委组织的“援滇扶贫”任务，荣获复旦大学附属肿瘤医院“对口支援工作杰出贡献奖”。" },
                            new Doctor{  Name=ChineseHelper.GetRandomChineseName(), Title="主治医师", Expert="肺癌、乳腺癌，胃癌的化疗，靶向治疗，内分泌治疗，免疫治疗，联合中医药治疗，癌痛治疗", Academic="复旦大学肿瘤学专业，医学博士，副主任医师", Introduction="，男，38岁，复旦大学肿瘤学专业，医学博士，副主任医师。从事肿瘤内科临床工作15年，熟悉常见肿瘤的诊治原则，擅长结直肠癌、胃癌和肺癌的系统性化疗和靶向治疗。        主要从事肿瘤分子靶向治疗和耐药领域的基础、临床和转化性研究，参与国家“十一五重大攻关计划分课题：分子靶向药物评价平台的建设”、“十二五科技重大专项：国际标准抗肿瘤新药临床试验平台建设”等重大专项科研项目，并主持国家自然科学基金面上项目、上海市自然科学基金项目。先后在国内外发表研究论文20余篇，代表性研究成果发表于“Clinical Cancer Research”，“BMC Medicine”，“Oncotarget”等国际知名学术期刊。        2015年参与完成“胃癌发生发展的分子机制研究与临床治疗新策略的建立和应用”项目，荣获中华人民共和国教育部“科学技术进步奖二等奖”。        2016年参加上海市卫计委组织的“援滇扶贫”任务，荣获复旦大学附属肿瘤医院“对口支援工作杰出贡献奖”。" },
                            new Doctor{  Name=ChineseHelper.GetRandomChineseName(), Title="主治医师", Expert="肺癌、乳腺癌，胃癌的化疗，靶向治疗，内分泌治疗，免疫治疗，联合中医药治疗，癌痛治疗", Academic="复旦大学肿瘤学专业，医学博士，副主任医师", Introduction="，男，38岁，复旦大学肿瘤学专业，医学博士，副主任医师。从事肿瘤内科临床工作15年，熟悉常见肿瘤的诊治原则，擅长结直肠癌、胃癌和肺癌的系统性化疗和靶向治疗。        主要从事肿瘤分子靶向治疗和耐药领域的基础、临床和转化性研究，参与国家“十一五重大攻关计划分课题：分子靶向药物评价平台的建设”、“十二五科技重大专项：国际标准抗肿瘤新药临床试验平台建设”等重大专项科研项目，并主持国家自然科学基金面上项目、上海市自然科学基金项目。先后在国内外发表研究论文20余篇，代表性研究成果发表于“Clinical Cancer Research”，“BMC Medicine”，“Oncotarget”等国际知名学术期刊。        2015年参与完成“胃癌发生发展的分子机制研究与临床治疗新策略的建立和应用”项目，荣获中华人民共和国教育部“科学技术进步奖二等奖”。        2016年参加上海市卫计委组织的“援滇扶贫”任务，荣获复旦大学附属肿瘤医院“对口支援工作杰出贡献奖”。" },
                            new Doctor{  Name=ChineseHelper.GetRandomChineseName(), Title="主治医师", Expert="肺癌、乳腺癌，胃癌的化疗，靶向治疗，内分泌治疗，免疫治疗，联合中医药治疗，癌痛治疗", Academic="复旦大学肿瘤学专业，医学博士，副主任医师", Introduction="，男，38岁，复旦大学肿瘤学专业，医学博士，副主任医师。从事肿瘤内科临床工作15年，熟悉常见肿瘤的诊治原则，擅长结直肠癌、胃癌和肺癌的系统性化疗和靶向治疗。        主要从事肿瘤分子靶向治疗和耐药领域的基础、临床和转化性研究，参与国家“十一五重大攻关计划分课题：分子靶向药物评价平台的建设”、“十二五科技重大专项：国际标准抗肿瘤新药临床试验平台建设”等重大专项科研项目，并主持国家自然科学基金面上项目、上海市自然科学基金项目。先后在国内外发表研究论文20余篇，代表性研究成果发表于“Clinical Cancer Research”，“BMC Medicine”，“Oncotarget”等国际知名学术期刊。        2015年参与完成“胃癌发生发展的分子机制研究与临床治疗新策略的建立和应用”项目，荣获中华人民共和国教育部“科学技术进步奖二等奖”。        2016年参加上海市卫计委组织的“援滇扶贫”任务，荣获复旦大学附属肿瘤医院“对口支援工作杰出贡献奖”。" },
                            new Doctor{  Name=ChineseHelper.GetRandomChineseName(), Title="主治医师", Expert="肺癌、乳腺癌，胃癌的化疗，靶向治疗，内分泌治疗，免疫治疗，联合中医药治疗，癌痛治疗", Academic="复旦大学肿瘤学专业，医学博士，副主任医师", Introduction="，男，38岁，复旦大学肿瘤学专业，医学博士，副主任医师。从事肿瘤内科临床工作15年，熟悉常见肿瘤的诊治原则，擅长结直肠癌、胃癌和肺癌的系统性化疗和靶向治疗。        主要从事肿瘤分子靶向治疗和耐药领域的基础、临床和转化性研究，参与国家“十一五重大攻关计划分课题：分子靶向药物评价平台的建设”、“十二五科技重大专项：国际标准抗肿瘤新药临床试验平台建设”等重大专项科研项目，并主持国家自然科学基金面上项目、上海市自然科学基金项目。先后在国内外发表研究论文20余篇，代表性研究成果发表于“Clinical Cancer Research”，“BMC Medicine”，“Oncotarget”等国际知名学术期刊。        2015年参与完成“胃癌发生发展的分子机制研究与临床治疗新策略的建立和应用”项目，荣获中华人民共和国教育部“科学技术进步奖二等奖”。        2016年参加上海市卫计委组织的“援滇扶贫”任务，荣获复旦大学附属肿瘤医院“对口支援工作杰出贡献奖”。" },
                            new Doctor{  Name=ChineseHelper.GetRandomChineseName(), Title="主治医师", Expert="肺癌、乳腺癌，胃癌的化疗，靶向治疗，内分泌治疗，免疫治疗，联合中医药治疗，癌痛治疗", Academic="复旦大学肿瘤学专业，医学博士，副主任医师", Introduction="，男，38岁，复旦大学肿瘤学专业，医学博士，副主任医师。从事肿瘤内科临床工作15年，熟悉常见肿瘤的诊治原则，擅长结直肠癌、胃癌和肺癌的系统性化疗和靶向治疗。        主要从事肿瘤分子靶向治疗和耐药领域的基础、临床和转化性研究，参与国家“十一五重大攻关计划分课题：分子靶向药物评价平台的建设”、“十二五科技重大专项：国际标准抗肿瘤新药临床试验平台建设”等重大专项科研项目，并主持国家自然科学基金面上项目、上海市自然科学基金项目。先后在国内外发表研究论文20余篇，代表性研究成果发表于“Clinical Cancer Research”，“BMC Medicine”，“Oncotarget”等国际知名学术期刊。        2015年参与完成“胃癌发生发展的分子机制研究与临床治疗新策略的建立和应用”项目，荣获中华人民共和国教育部“科学技术进步奖二等奖”。        2016年参加上海市卫计委组织的“援滇扶贫”任务，荣获复旦大学附属肿瘤医院“对口支援工作杰出贡献奖”。" },

                        }
                    });
                }
            }
        }
    }
}
