using Application.Repositories;
using Core.Application.Rules;
using Core.Security.Entities;
using Domain.Entities;

namespace Application.Features.Tests.Rules
{
    public class TestBusinessRules : BaseBusinessRules
    {
        private readonly ITestRepository _testRepository;

        public TestBusinessRules(ITestRepository testRepository)
        {
            _testRepository = testRepository;
        }

        /// <summary>
        /// Checks if test exists by ID
        /// </summary>
        public async Task TestShouldExistWhenSelected(int id)
        {
            Test? test = await _testRepository.GetAsync(t => t.Id == id && !t.IsDeleted);
            CheckEntityExists(test, "Test not found.");
        }

        /// <summary>
        /// Checks if test name is unique (for create)
        /// </summary>
        public async Task TestNameShouldBeUnique(string name)
        {
            await CheckPropertyIsUniqueWithAny<Test, ITestRepository>(
                _testRepository,
                t => t.Name == name && !t.IsDeleted,
                "A test with this name already exists."
            );
        }

        /// <summary>
        /// Checks if test name is unique (for update)
        /// </summary>
        public async Task TestNameShouldBeUniqueForUpdate(string name, int exceptId)
        {
            await CheckPropertyIsUniqueExcept<Test, ITestRepository>(
                _testRepository,
                t => t.Name == name && !t.IsDeleted,
                exceptId,
                "A test with this name already exists."
            );
        }

        /// <summary>
        /// Checks if test is not soft deleted
        /// </summary>
        public void TestShouldNotBeDeleted(Test test)
        {
            CheckEntityNotDeleted(test, "Test has been deleted.");
        }

        /// <summary>
        /// Validates test name format
        /// </summary>
        public void TestNameShouldBeValid(string name)
        {
            CheckStringNotEmpty(name, "Test Name");
            CheckStringLength(name, 2, 100, "Test Name");
        }

        /// <summary>
        /// Checks if entity exists     
        /// </summary>
        public void CheckEntityExists(Test? test)
        {
            CheckEntityExists(test, "Test not found.");
        }
    }
}