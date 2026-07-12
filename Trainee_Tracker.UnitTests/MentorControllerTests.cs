using Trainee_Tracker.Repositories;
using Xunit;
using Moq;
using Trainee_Tracker.Data.Rejections;
using Trainee_Tracker.Data.LessonAssignments;
using Trainee_Tracker.Controllers;
using Trainee_Tracker.Data.TraineeRepository;
using Trainee_Tracker.Data.MentorRepository;
using Trainee_Tracker.Models;
using Microsoft.AspNetCore.Mvc;
using Trainee_Tracker.Services;


namespace Trainee_Tracker.UnitTests;

public class MentorControllerTests
{
    private readonly Mock<IUserRepository> _userRepoMock;
    private readonly Mock<IRejectionRepository> _rejectionRepoMock;
    private readonly Mock<ILessonAssignmentRepository> _assignmentRepoMock;
    private readonly Mock<ITraineeRepository> _traineeRepoMock;
    private readonly Mock<IMentorRepository> _mentorRepoMock;
    private readonly Mock<IProgressService> _progressServiceMock;
    private readonly Mock<WorkingHoursService> _workingHoursServiceMock;
    private readonly MentorController _controller;

    public MentorControllerTests()
    {
        _userRepoMock = new Mock<IUserRepository>();
        _assignmentRepoMock = new Mock<ILessonAssignmentRepository>();
        _rejectionRepoMock = new Mock<IRejectionRepository>();
        _traineeRepoMock = new Mock<ITraineeRepository>();
        _mentorRepoMock = new Mock<IMentorRepository>();
        _progressServiceMock = new Mock<IProgressService>();
        _workingHoursServiceMock = new Mock<WorkingHoursService>();

        _controller = new MentorController(_mentorRepoMock.Object, _userRepoMock.Object, _assignmentRepoMock.Object, _rejectionRepoMock.Object, _traineeRepoMock.Object, _workingHoursServiceMock.Object, _progressServiceMock.Object);
    }

    //code-owner: Julia Sandner
    //unit-test for Mentor accepting an assignment for a trainee (successful)
    [Fact]
    public void Accep_ValidAssignment_UpdateStatusAndRedirectToOverview()
    {
        // Arrange
        var assignment = new LessonAssignment{Id = 5, TraineeId = 41};
        _assignmentRepoMock.Setup(r => r.GetById(5)).Returns(assignment);

        //Act
        var result = _controller.Accept(5);

        //Assert
        _assignmentRepoMock.Verify(r => r.UpdateStatus(5, LessonAssignmentStatus.Accepted), Times.Once);
        var redirect = Assert.IsType<RedirectToActionResult>(result);
        Assert.Equal("AssignmentOverview", redirect.ActionName);
        Assert.Equal(41, redirect.RouteValues["traineeId"]);

    }

    //Code-Owner: Julia Sandner
    //unit-test for Mentor accepting an unknown assignment for a trainee 
    [Fact]
    public void Accept_UnknowenAssignment_ReturnNotFound()
    {
        //Arrange
        _assignmentRepoMock.Setup(r => r.GetById(It.IsAny<int>())).Returns((LessonAssignment?) null);

        //Act
        var result = _controller.Accept(999);

        //Assert
        Assert.IsType<NotFoundResult>(result);
        _assignmentRepoMock.Verify(r => r.UpdateStatus(It.IsAny<int>(), It.IsAny<LessonAssignmentStatus>()), Times.Never);

    }
    
    //Code-Owner: Julia Sandner
    //unit-test for Mentor skipping an assignment for a trainee (successful)
    [Fact]
    public void Skip_ValidAssignment_UpdateStatusAndRedirectToOverview()
    {
        //Arrange
        var assignment = new LessonAssignment{Id = 7, TraineeId = 42};
        _assignmentRepoMock.Setup(r => r.GetById(7)).Returns(assignment);

        //Act
        var result = _controller.SkipAssignment(7);

        //Assert
        _assignmentRepoMock.Verify(r => r.UpdateStatus(7, LessonAssignmentStatus.Skipped), Times.Once);
        var redirect = Assert.IsType<RedirectToActionResult>(result);
        Assert.Equal("AssignmentOverview", redirect.ActionName);
        Assert.Equal(42, redirect.RouteValues["traineeId"]);
    }

    //Code-Owner: Julia Sandner
    //unit-test for mentor skipping an unknown assignment for a trainee
    [Fact]
    public void Skip_UnknownAssignment_ReturnNull()
    {   
        //Arrange
        _assignmentRepoMock.Setup(r => r.GetById(It.IsAny<int>())).Returns((LessonAssignment?) null);

        //Act
        var result = _controller.SkipAssignment(999);
    
        //Assert
        Assert.IsType<NotFoundResult>(result);
    }

    //code-owner: Julia Sandner
    //unit-test for mentor reordering Assignments for a trainee
    [Fact]
    public void UpdateAssignmentPosition_CallRepositoryWithGivenOrderAndRedirect()
    {
        //Arrange
        var orderedIds = new List<int> {2, 1, 3};

        //Act
        var result = _controller.UpdateAssignmentOrder(42, orderedIds);

        //Assert
        _assignmentRepoMock.Verify(r => r.UpdateAssignmentPositions(orderedIds), Times.Once);

        var redirect = Assert.IsType<RedirectToActionResult>(result);
        Assert.Equal("AssignmentOverview", redirect.ActionName);
        Assert.Equal(42, redirect.RouteValues["traineeId"]);
    }

    //Code-owner: Julia Sandner
    //unit-test for mentor reordering assignments without new order for a trainee
    [Fact]
    public void UpdateAssignmentPosition_EmptyList_StillCallRepository()
    {
        //Arrange
        var orderedIds = new List<int>();

        //Act
        var result = _controller.UpdateAssignmentOrder(42, orderedIds);

        //Assert
        _assignmentRepoMock.Verify(r => r.UpdateAssignmentPositions(orderedIds), Times.Once);
        Assert.IsType<RedirectToActionResult>(result);
    }

}