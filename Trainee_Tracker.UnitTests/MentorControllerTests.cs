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
using Moq.Protected;
using System.Runtime.CompilerServices;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Http;


namespace Trainee_Tracker.UnitTests;

public class MentorControllerTests
{
    private readonly Mock<IUserRepository> _userRepoMock;
    private readonly Mock<IRejectionRepository> _rejectionRepoMock;
    private readonly Mock<ILessonAssignmentRepository> _assignmentRepoMock;
    private readonly Mock<ITraineeRepository> _traineeRepoMock;
    private readonly Mock<IMentorRepository> _mentorRepoMock;
    private readonly Mock<IProgressService> _progressServiceMock;
    private readonly Mock<IAssignmentService> _assignmentServiceMock;
    private readonly WorkingHoursService _workingHoursService;
    private readonly MentorController _controller;

    public MentorControllerTests()
    {
        _userRepoMock = new Mock<IUserRepository>();
        _assignmentRepoMock = new Mock<ILessonAssignmentRepository>();
        _rejectionRepoMock = new Mock<IRejectionRepository>();
        _traineeRepoMock = new Mock<ITraineeRepository>();
        _mentorRepoMock = new Mock<IMentorRepository>();
        _progressServiceMock = new Mock<IProgressService>();
        _assignmentServiceMock = new Mock<IAssignmentService>();
        
        var handlerMock = new Mock<HttpMessageHandler>();
        handlerMock
        .Protected()
        .Setup<Task<HttpResponseMessage>>("SendAsync", ItExpr.IsAny<HttpRequestMessage>(), ItExpr.IsAny<CancellationToken>())
        .ReturnsAsync(new HttpResponseMessage{ StatusCode = System.Net.HttpStatusCode.OK, Content = new StringContent("""{"entries"}:[]}""")});

        var httpClient = new HttpClient(handlerMock.Object)
        {
            BaseAddress = new Uri("http://fake-api.local/")
        };

        _workingHoursService = new WorkingHoursService(httpClient);

        // Curriculum Repo und Service sind null, da sie für die aktuellen Tests nicht benötigt werden
        _controller = new MentorController(_mentorRepoMock.Object, _userRepoMock.Object, _assignmentRepoMock.Object, _rejectionRepoMock.Object, _traineeRepoMock.Object, _workingHoursService, _progressServiceMock.Object, null, null, _assignmentServiceMock.Object);

        _controller.TempData = new TempDataDictionary(new DefaultHttpContext(), Mock.Of<ITempDataProvider>());
    }

    //code-owner: Julia Sandner
    //unit-test for Mentor accepting an assignment for a trainee (successful)
    [Fact]
    public void Accept_ValidAssignment_UpdateStatusAndRedirectToOverview()
    {
        // Arrange
        var assignment = new LessonAssignment{Id = 5, TraineeId = 41};
        _assignmentRepoMock.Setup(r => r.GetById(5)).Returns(assignment);
        _assignmentServiceMock.Setup(s => s.UpdateAssignmentStatus(5, LessonAssignmentStatus.Accepted)).Returns(assignment);

        //Act
        var result = _controller.Accept(5);

        //Assert
        _assignmentServiceMock.Verify(r => r.UpdateAssignmentStatus(5, LessonAssignmentStatus.Accepted), Times.Once);
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
        _assignmentServiceMock.Verify(s => s.UpdateAssignmentStatus(It.IsAny<int>(), It.IsAny<LessonAssignmentStatus>()), Times.Never);
    }

    //Code-Owner: Julia Sandner
    //unit-test for Mentor accepting an assignmnet that is in an invalid status for this transition
    [Fact]
    public void Accept_InvalidTransition_SetsErrorAndRedirectsToOverview()
    {
        //Arrange
        var assignment = new LessonAssignment{Id = 5, TraineeId = 41, Status = LessonAssignmentStatus.Open};
        _assignmentRepoMock.Setup(r => r.GetById(5)).Returns(assignment);
        _assignmentServiceMock.Setup(s => s.UpdateAssignmentStatus(5, LessonAssignmentStatus.Accepted)).Throws(new InvalidOperationException("Invalid status transition for assignment 5: Open -> Accepted."));

        //Act
        var result = _controller.Accept(5);

        //Assert
        var redirect =Assert.IsType<RedirectToActionResult>(result);
        Assert.Equal("AssignmentOverview", redirect.ActionName);
        Assert.Equal(41, redirect.RouteValues["traineeId"]);
        Assert.NotNull(_controller.TempData["Error"]);
    }
    
    //Code-Owner: Julia Sandner
    //unit-test for Mentor skipping an assignment for a trainee (successful)
    [Fact]
    public void Skip_ValidAssignment_UpdateStatusAndRedirectToOverview()
    {
        //Arrange
        var assignment = new LessonAssignment{Id = 7, TraineeId = 42};
        _assignmentRepoMock.Setup(r => r.GetById(7)).Returns(assignment);
        _assignmentServiceMock.Setup( s=> s.UpdateAssignmentStatus(7, LessonAssignmentStatus.Skipped)).Returns(assignment);

        //Act
        var result = _controller.SkipAssignment(7);

        //Assert
        _assignmentServiceMock.Verify(r => r.UpdateAssignmentStatus(7, LessonAssignmentStatus.Skipped), Times.Once);
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
        _assignmentServiceMock.Verify(s=> s.UpdateAssignmentStatus(It.IsAny<int>(), It.IsAny<LessonAssignmentStatus>()), Times.Never);
    }

    //code-owner: Julia Sandner
    //unit-test for mentor reordering Assignments for a trainee
    [Fact]
    public void UpdateAssignmentPosition_CallServiceWithGivenOrderAndRedirect()
    {
        //Arrange
        var orderedIds = new List<int> {2, 1, 3};

        //Act
        var result = _controller.UpdateAssignmentOrder(42, orderedIds);

        //Assert
        _assignmentServiceMock.Verify(r => r.UpdateAssignmentOrder(orderedIds), Times.Once);

        var redirect = Assert.IsType<RedirectToActionResult>(result);
        Assert.Equal("AssignmentOverview", redirect.ActionName);
        Assert.Equal(42, redirect.RouteValues["traineeId"]);
    }

    //Code-owner: Julia Sandner
    //unit-test for mentor reordering assignments without new order for a trainee
    [Fact]
    public void UpdateAssignmentPosition_EmptyList_StillCallService()
    {
        //Arrange
        var orderedIds = new List<int>();

        //Act
        var result = _controller.UpdateAssignmentOrder(42, orderedIds);

        //Assert
        _assignmentServiceMock.Verify(r => r.UpdateAssignmentOrder(orderedIds), Times.Once);
        Assert.IsType<RedirectToActionResult>(result);
    }

    //Code-Owner: Julia Sandner
    //unit-test for mentor rejecting an assignmnet with a valid reason
    [Fact]
    public void Reject_ValidAssignmentAndReason_CallServiceAndRedirect()
    {
        //Arrange
        var assignment = new LessonAssignment{Id = 8, TraineeId = 44};
        _assignmentRepoMock.Setup(r => r.GetById(8)).Returns(assignment);

        //Act
        var result = _controller.RejectAssignment(8, "not detailed enough");

        //Assert
        _assignmentServiceMock.Verify(s => s.RejectAssignment(8, "not detailed enough"), Times.Once);
        var redirect = Assert.IsType<RedirectToActionResult>(result);
        Assert.Equal("AssignmentOverview", redirect.ActionName);
        Assert.Equal(44, redirect.RouteValues["traineeId"]);
    }

    //code-owner: Julia Sandner
    //unit-test for mentor rejecting an unknown assignment
    [Fact]
    public void Reject_UnknownAssignment_ReturnNotFound()
    {
        //Arrange
        _assignmentRepoMock.Setup(r => r.GetById(It.IsAny<int>())).Returns((LessonAssignment?) null);

        //Act
        var result = _controller.RejectAssignment(999, "some reasons");

        //Assert
        Assert.IsType<NotFoundResult>(result);
        _assignmentServiceMock.Verify(s => s.RejectAssignment(It.IsAny<int>(), It.IsAny<string>()), Times.Never);

    }

    //Code-Owner: Julia Sandner
    //unittest for mentor rejectiong an assignment with an empty reason
    [Fact]
    public void Reject_EmptyReason_SetsErrorAndRedirectsToOverview()
    {
        //Arrange
        var assignment = new LessonAssignment{Id = 8, TraineeId = 44};
        _assignmentRepoMock.Setup(r => r.GetById(8)).Returns(assignment);
        _assignmentServiceMock.Setup( s => s.RejectAssignment(8, "")).Throws(new ArgumentException("A Reason is required.", "reason"));

        //Act
        var result = _controller.RejectAssignment(8, "");

        //Assert
        var redirect = Assert.IsType<RedirectToActionResult>(result);
        Assert.Equal("AssignmentOverview", redirect.ActionName);
        Assert.Equal(44, redirect.RouteValues["traineeId"]);
        Assert.NotNull(_controller.TempData["Error"]);
    }

    //Code-Owner: Julia Sandner
    // unit-test for mentor rejectiong an assignment that is not finished
    [Fact]
    public void Reject_InvalidTransition_SetsErrorAndRedirectsToOverview()
    {
        //Arrange
        var assignment = new LessonAssignment{Id = 9, TraineeId = 40, Status = LessonAssignmentStatus.Open};
        _assignmentRepoMock.Setup(r => r.GetById(9)).Returns(assignment);
        _assignmentServiceMock.Setup( s => s.RejectAssignment(9, "some reasons")).Throws(new InvalidOperationException("Invalid status transition for assignment 9: Open -> rejected"));

        //Act
        var result = _controller.RejectAssignment(9, "some reasons");

        //Assert
        var redirect = Assert.IsType<RedirectToActionResult>(result);
        Assert.Equal("AssignmentOverview", redirect.ActionName);
        Assert.Equal(40, redirect.RouteValues["traineeId"]);
        Assert.NotNull(_controller.TempData["Error"]);
    }

}