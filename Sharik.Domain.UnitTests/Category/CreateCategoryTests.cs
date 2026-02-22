using FluentAssertions;
using Sharik.Domain.Skills;
using Sharik.Domain.Skills.SkillCategories;

namespace Sharik.Domain.UnitTests.Category
{
    public sealed class CreateCategoryTests
    {

        [Fact]
        public void Create_WithValidData_ShouldSucceed()
        {
            var name = "Programming";

            var result = SkillCategory.Create(name);

            result.IsSuccess.Should().BeTrue();
            result.Value.Name.Should().Be(name);
        }

        [Fact]
        public void Create_WhenNameTooShort_ShouldFail()
        {
            var name = "Pg";

            var result = SkillCategory.Create(name);

            result.IsFailure.Should().BeTrue();
            result.Errors.Should().ContainSingle(e => e.Code == SkillCategoryErrors.SkillCategoryNameTooShort.Code);
        }

        [Fact]
        public void Create_WhenNameTooLong_ShouldFail()
        {
            var name = new string('x' , 101);

            var result = SkillCategory.Create(name);

            result.IsFailure.Should().BeTrue();
            result.Errors.Should().ContainSingle(e => e.Code == SkillCategoryErrors.SkillCategoryNameTooLong.Code);
        }

        [Theory]
        [InlineData(null)]
        [InlineData("  ")]
        public void Create_WhenNameIsNullOrWhiteSpace_ShouldFail(string? invalidName)
        {

            var name = invalidName;

            var result = SkillCategory.Create(name);


            result.IsFailure.Should().BeTrue();
            result.Errors.Should().ContainSingle(e => e.Code == SkillCategoryErrors.SkillCategoryNameRequired.Code);

        }

        [Fact]
        public void AddSkill_NewSkill_SkillAdded()
        {
            var category = SkillCategory.Create("Music").Value;
            var skillName = "Guitar";

            var result = category.AddSkill(skillName);

            result.IsSuccess.Should().BeTrue();
            category.Skills.Should().ContainSingle(s => s.Name == skillName);
        }

        [Fact]
        public void AddSkill_SkillAlreadyExists_ShouldFail()
        {
            var category = SkillCategory.Create("Music").Value;
            category.AddSkill("Guitar");

            var result = category.AddSkill("Guitar");

            result.IsFailure.Should().BeTrue();
            result.Errors.Should().ContainSingle(e => e.Code == SkillErrors.SkillNameIsAlreadyExists.Code);
        }

    }
}
