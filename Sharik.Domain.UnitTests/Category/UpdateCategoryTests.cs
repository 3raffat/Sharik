using FluentAssertions;
using Sharik.Domain.Skills;
using Sharik.Domain.Skills.SkillCategories;

namespace Sharik.Domain.UnitTests.Category
{
    public sealed class UpdateCategoryTests
    {

        [Fact]
        public void Update_WithValidData_ShouldSucceed()
        {
            var oldName = "Old Name";
            var category = SkillCategory.Create(oldName).Value;
            var newName = "New Name";

            var result = category.Update(newName);

            result.IsSuccess.Should().BeTrue();
            category.Name.Should().Be(newName);
        }

        [Fact]
        public void Update_WhenNameTooShort_ShouldFail()
        {

            var oldName = "Old Name";
            var category = SkillCategory.Create(oldName).Value;
            var newName = "N";

            var result = category.Update(newName);


            result.IsFailure.Should().BeTrue();
            result.Errors.Should().ContainSingle(e => e.Code == SkillCategoryErrors.SkillCategoryNameTooShort.Code);
        }

        [Fact]
        public void Update_WhenNameTooLong_ShouldFail()
        {

            var oldName = "Old Name";
            var category = SkillCategory.Create(oldName).Value;
            var newName = new string('x' , 101);

            var result = category.Update(newName);

            result.IsFailure.Should().BeTrue();
            result.Errors.Should().ContainSingle(e => e.Code == SkillCategoryErrors.SkillCategoryNameTooLong.Code);
        }

        [Theory]
        [InlineData(null)]
        [InlineData("  ")]
        public void Update_WhenNameIsNullOrWhiteSpace_ShouldFail(string? invalidName)
        {
            var name = "Csharp";
            var category = SkillCategory.Create(name).Value;
            var newName = invalidName;

            var result = category.Update(newName);

            result.IsFailure.Should().BeTrue();
            result.Errors.Should().ContainSingle(e => e.Code == SkillCategoryErrors.SkillCategoryNameRequired.Code);

        }

        [Fact]
        public void UpdateSkill_SkillExistsAndNameIsUnique_SkillUpdated()
        {
            var category = SkillCategory.Create("Cooking").Value;
            var skill = category.AddSkill("Baking").Value;
            var newName = "Pastry Baking";

            var result = category.UpdateSkill(skill.Id , newName);

            result.IsSuccess.Should().BeTrue();
            skill.Name.Should().Be(newName);
        }
    }
}
