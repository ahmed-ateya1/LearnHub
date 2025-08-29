using Course.API.Controllers;
using Course.Application.Dtos.CourseDto;
using Course.Application.Slices.Courses.Commands.CreateCourse;
using Course.Application.Slices.Courses.Commands.DeleteCourse;
using Course.Application.Slices.Courses.Commands.UpdateCourse;
using Course.Application.Slices.Courses.Commands.UpdateCourseStatus;
using Course.Application.Slices.Courses.Queries.GetAllCourses;
using Course.Application.Slices.Courses.Queries.GetCourseById;
using Course.Application.Slices.Courses.Queries.GetCOurseByName;
using Course.Application.Slices.Courses.Queries.GetCoursesByCategoryId;
using Course.Application.Slices.Courses.Queries.GetCoursesByUserId;
using Course.Domain.Enums;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;

namespace CourseService.Test
{
    public class CourseControllerTests
    {
        private static CourseController CreateController(Func<object, object?> responder)
        {
            var sender = new TestSender(responder);
            return new CourseController(sender);
        }

        [Fact]
        public async Task CreateCourse_ReturnsOk_WhenSenderReturnsValidResponse()
        {
            // Arrange
            var expectedResponse = new { Id = Guid.NewGuid(), Title = "Test Course" };
            var controller = CreateController(req => req is CreateCourseCommand ? expectedResponse : null);
            var request = new CourseAddRequest
            {
                Title = "Test",
                Description = "Desc",
                Price = 10m,
                Poster = CreateFormFile("poster.jpg"),
                InstructorId = Guid.NewGuid(),
                CourseLevel = CourseLevel.Beginner,
                Language = "en",
                CategoryId = Guid.NewGuid()
            };

            // Act
            var result = await controller.CreateCourse(request);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            Assert.Equal(expectedResponse, okResult.Value);
        }

        [Fact]
        public async Task CreateCourse_ReturnsBadRequest_WhenSenderReturnsNull()
        {
            // Arrange
            var controller = CreateController(_ => null);
            var request = new CourseAddRequest
            {
                Title = "Test",
                Description = "Desc",
                Price = 10m,
                Poster = CreateFormFile("poster.jpg"),
                InstructorId = Guid.NewGuid(),
                CourseLevel = CourseLevel.Beginner,
                Language = "en",
                CategoryId = Guid.NewGuid()
            };

            // Act
            var result = await controller.CreateCourse(request);

            // Assert
            var badRequest = Assert.IsType<BadRequestObjectResult>(result);
            Assert.Equal("Failed to create course.", badRequest.Value);
        }

        [Fact]
        public async Task UpdateCourse_ReturnsOk_WhenSenderReturnsValidResponse()
        {
            // Arrange
            var expectedResponse = new { Id = Guid.NewGuid(), Title = "Updated Course" };
            var controller = CreateController(req => req is UpdateCourseCommand ? expectedResponse : null);
            var request = new CourseUpdateRequest
            {
                Id = Guid.NewGuid(),
                Title = "Updated",
                Description = "Updated Desc",
                Price = 20m,
                Poster = null,
                CourseLevel = CourseLevel.Intermediate,
                Language = "en",
                CourseStatus = CourseStatus.Published,
                CategoryId = Guid.NewGuid()
            };

            // Act
            var result = await controller.UpdateCourse(request);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            Assert.Equal(expectedResponse, okResult.Value);
        }

        [Fact]
        public async Task UpdateCourse_ReturnsBadRequest_WhenSenderReturnsNull()
        {
            // Arrange
            var controller = CreateController(_ => null);
            var request = new CourseUpdateRequest
            {
                Id = Guid.NewGuid(),
                Title = "Updated",
                Description = "Updated Desc",
                Price = 20m,
                Poster = null,
                CourseLevel = CourseLevel.Intermediate,
                Language = "en",
                CourseStatus = CourseStatus.Published,
                CategoryId = Guid.NewGuid()
            };

            // Act
            var result = await controller.UpdateCourse(request);

            // Assert
            var badRequest = Assert.IsType<BadRequestObjectResult>(result);
            Assert.Equal("Failed to update course.", badRequest.Value);
        }

        [Fact]
        public async Task DeleteCourse_ReturnsOk_WhenSenderReturnsTrue()
        {
            // Arrange
            var controller = CreateController(req => req is DeleteCourseCommand ? true : null);
            var courseId = Guid.NewGuid();

            // Act
            var result = await controller.DeleteCourse(courseId);

            // Assert
            var ok = Assert.IsType<OkObjectResult>(result);
            Assert.Contains(courseId.ToString(), ok.Value?.ToString());
        }

        [Fact]
        public async Task DeleteCourse_ReturnsBadRequest_WhenSenderReturnsFalse()
        {
            // Arrange
            var controller = CreateController(req => req is DeleteCourseCommand ? false : null);
            var courseId = Guid.NewGuid();

            // Act
            var result = await controller.DeleteCourse(courseId);

            // Assert
            var badRequest = Assert.IsType<BadRequestObjectResult>(result);
            Assert.Contains(courseId.ToString(), badRequest.Value?.ToString());
        }

        [Fact]
        public async Task GetAllCourses_ReturnsOk_WhenSenderReturnsNonEmptyList()
        {
            // Arrange
            var expectedCourses = new[] { new { Id = Guid.NewGuid(), Title = "Course 1" } };
            var controller = CreateController(req => req is GetAllCoursesQuery ? expectedCourses : null);

            // Act
            var result = await controller.GetAllCourses();

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            Assert.Equal(expectedCourses, okResult.Value);
        }

        [Fact]
        public async Task GetAllCourses_ReturnsNotFound_WhenSenderReturnsEmptyList()
        {
            // Arrange
            var controller = CreateController(req => req is GetAllCoursesQuery ? Array.Empty<object>() : null);

            // Act
            var result = await controller.GetAllCourses();

            // Assert
            var notFound = Assert.IsType<NotFoundObjectResult>(result);
            Assert.Equal("No courses found.", notFound.Value);
        }

        [Fact]
        public async Task GetCourseById_ReturnsOk_WhenSenderReturnsValidCourse()
        {
            // Arrange
            var courseId = Guid.NewGuid();
            var expectedCourse = new { Id = courseId, Title = "Found Course" };
            var controller = CreateController(req => req is GetCourseByIdQuery ? expectedCourse : null);

            // Act
            var result = await controller.GetCourseById(courseId);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            Assert.Equal(expectedCourse, okResult.Value);
        }

        [Fact]
        public async Task GetCourseById_ReturnsNotFound_WhenSenderReturnsNull()
        {
            // Arrange
            var courseId = Guid.NewGuid();
            var controller = CreateController(_ => null);

            // Act
            var result = await controller.GetCourseById(courseId);

            // Assert
            var notFound = Assert.IsType<NotFoundObjectResult>(result);
            Assert.Contains(courseId.ToString(), notFound.Value?.ToString());
        }

        [Fact]
        public async Task GetCourseByName_ReturnsOk_WhenSenderReturnsNonEmptyList()
        {
            // Arrange
            var courseName = "Test Course";
            var expectedCourses = new[] { new { Id = Guid.NewGuid(), Title = courseName } };
            var controller = CreateController(req => req is GetCourseByNameQuery ? expectedCourses : null);

            // Act
            var result = await controller.GetCourseByName(courseName);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            Assert.Equal(expectedCourses, okResult.Value);
        }

        [Fact]
        public async Task GetCourseByName_ReturnsNotFound_WhenSenderReturnsEmptyList()
        {
            // Arrange
            var courseName = "Non-existent Course";
            var controller = CreateController(req => req is GetCourseByNameQuery ? Array.Empty<object>() : null);

            // Act
            var result = await controller.GetCourseByName(courseName);

            // Assert
            var notFound = Assert.IsType<NotFoundObjectResult>(result);
            Assert.Contains(courseName, notFound.Value?.ToString());
        }

        [Fact]
        public async Task GetCoursesByInstructorId_ReturnsOk_WhenSenderReturnsNonEmptyList()
        {
            // Arrange
            var instructorId = Guid.NewGuid();
            var expectedCourses = new[] { new { Id = Guid.NewGuid(), InstructorId = instructorId } };
            var controller = CreateController(req => req is GetCoursesByUserIdQuery ? expectedCourses : null);

            // Act
            var result = await controller.GetCoursesByInstructorId(instructorId);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            Assert.Equal(expectedCourses, okResult.Value);
        }

        [Fact]
        public async Task GetCoursesByInstructorId_ReturnsNotFound_WhenSenderReturnsEmptyList()
        {
            // Arrange
            var instructorId = Guid.NewGuid();
            var controller = CreateController(req => req is GetCoursesByUserIdQuery ? Array.Empty<object>() : null);

            // Act
            var result = await controller.GetCoursesByInstructorId(instructorId);

            // Assert
            var notFound = Assert.IsType<NotFoundObjectResult>(result);
            Assert.Contains(instructorId.ToString(), notFound.Value?.ToString());
        }

        [Fact]
        public async Task GetCoursesByCategoryId_ReturnsOk_WhenSenderReturnsNonEmptyList()
        {
            // Arrange
            var categoryId = Guid.NewGuid();
            var expectedCourses = new[] { new { Id = Guid.NewGuid(), CategoryId = categoryId } };
            var controller = CreateController(req => req is GetCoursesByCategoryQuery ? expectedCourses : null);

            // Act
            var result = await controller.GetCoursesByCategoryId(categoryId);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            Assert.Equal(expectedCourses, okResult.Value);
        }

        [Fact]
        public async Task GetCoursesByCategoryId_ReturnsNotFound_WhenSenderReturnsEmptyList()
        {
            // Arrange
            var categoryId = Guid.NewGuid();
            var controller = CreateController(req => req is GetCoursesByCategoryQuery ? Array.Empty<object>() : null);

            // Act
            var result = await controller.GetCoursesByCategoryId(categoryId);

            // Assert
            var notFound = Assert.IsType<NotFoundObjectResult>(result);
            Assert.Contains(categoryId.ToString(), notFound.Value?.ToString());
        }

        [Fact]
        public async Task UpdateCourseStatus_ReturnsOk_WhenSenderReturnsTrue()
        {
            // Arrange
            var controller = CreateController(req => req is UpdateCourseStatusCommand ? true : null);
            var courseId = Guid.NewGuid();

            // Act
            var result = await controller.UpdateCourseStatus(courseId, CourseStatus.Published);

            // Assert
            var ok = Assert.IsType<OkObjectResult>(result);
            Assert.Equal(true, ok.Value);
        }

        [Fact]
        public async Task UpdateCourseStatus_ReturnsBadRequest_WhenSenderReturnsFalse()
        {
            // Arrange
            var controller = CreateController(req => req is UpdateCourseStatusCommand ? false : null);
            var courseId = Guid.NewGuid();

            // Act
            var result = await controller.UpdateCourseStatus(courseId, CourseStatus.Published);

            // Assert
            var badRequest = Assert.IsType<BadRequestObjectResult>(result);
            Assert.Contains(courseId.ToString(), badRequest.Value?.ToString());
        }

        private static IFormFile CreateFormFile(string fileName)
        {
            var ms = new MemoryStream(new byte[] { 0x1, 0x2, 0x3 });
            return new FormFile(ms, 0, ms.Length, "Poster", fileName);
        }

    }
}