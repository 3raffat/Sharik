using FluentAssertions;
using Sharik.Domain.Skills;
using System;
using System.Collections.Generic;
using System.Text;

namespace Sharik.Domain.UnitTests.Skills
{
    public sealed class CreateSkillTests
    {
        private readonly Guid _guid = Guid.NewGuid();

        [Fact]
        public void Create_WithValidData_ShouldSucceed()
        {

            var categoryId = _guid;
            var name = "Csharp";

            var result = Skill.Create(categoryId, name);


            result.IsSuccess.Should().BeTrue(); 
            result.Value.Name.Should().Be(name);    
            result.Value.SkillCategoryId.Should().Be(categoryId);   
        }

        [Fact]
        public void Create_WhenNameTooShort_ShouldFail()
        {

            var categoryId = _guid;
            var name = "c#";

            var result = Skill.Create(categoryId , name);


            result.IsFailure.Should().BeTrue();
            result.Errors.Should().ContainSingle(e=>e.Code==SkillErrors.SkillNameTooShort.Code);
        }

        [Fact]
        public void Create_WhenNameTooLong_ShouldFail()
        {

            var categoryId = _guid;
            var name = new string('a' , 101); 

            var result = Skill.Create(categoryId , name);


            result.IsFailure.Should().BeTrue();
            result.Errors.Should().ContainSingle(e => e.Code == SkillErrors.SkillNameTooLong.Code);
        }

        [Theory]
        [InlineData(null)]
        [InlineData("  ")]
        public void Create_WhenNameIsNullOrWhiteSpace_ShouldFail(string? invalidName)
        {

            var categoryId = _guid;
            var name = invalidName;

            var result = Skill.Create(categoryId , name);


            result.IsFailure.Should().BeTrue();
            result.Errors.Should().ContainSingle(e => e.Code == SkillErrors.SkillNameRequired.Code);

        }


    }
}
