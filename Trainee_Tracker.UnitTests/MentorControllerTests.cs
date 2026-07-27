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
using System.Security.Claims;


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
    private readonly Mock<IWorkingHoursSyncService> _workingHoursSyncServiceMock;    private readonly MentorController _controller;

    public MentorControllerTests()
    {
        _userRepoMock = new Mock<IUserRepository>();
        _assignmentRepoMock = new Mock<ILessonAssignmentRepository>();
        _rejectionRepoMock = new Mock<IRejectionRepository>();
        _traineeRepoMock = new Mock<ITraineeRepository>();
        _mentorRepoMock = new Mock<IMentorRepository>();
        _progressServiceMock = new Mock<IProgressService>();
        _assignmentServiceMock = new Mock<IAssignmentService>();
        _workingHoursSyncServiceMock = new Mock<IWorkingHoursSyncService>();
        
        var handlerMock = new Mock<HttpMessageHandler>();
        handlerMock
        .Protected()
        .Setup<Task<HttpResponseMessage>>("SendAsync", ItExpr.IsAny<HttpRequestMessage>(), ItExpr.IsAny<CancellationToken>())
        .ReturnsAsync(new HttpResponseMessage{ StatusCode = System.Net.HttpStatusCode.OK, Content = new StringContent("""{"entries"}:[]}""")});

        var httpClient = new HttpClient(handlerMock.Object)
        {
            BaseAddress = new Uri("http://fake-api.local/")
        };


        // Curriculum Repo und Service sind null, da sie für die aktuellen Tests nicht benötigt werden
        _controller = new MentorController(_mentorRepoMock.Object, _userRepoMock.Object, _assignmentRepoMock.Object, _rejectionRepoMock.Object, _traineeRepoMock.Object, _progressServiceMock.Object, null, null, _assignmentServiceMock.Object, _workingHoursSyncServiceMock.Object);

        _controller.TempData = new TempDataDictionary(new DefaultHttpContext(), Mock.Of<ITempDataProvider>());
        SetUser(1);
    }

    //Code-Owner: Julia Sandner
    private void SetUser(int? mentorID)
    {
        var claims = new List<Claim>();
        if (mentorID.HasValue)
        {
            claims.Add(new Claim(ClaimTypes.NameIdentifier, mentorID.Value.ToString()));
        }
        var identity = new ClaimsIdentity(claims, mentorID.HasValue ? "TestAuthType" : null);
        _controller.ControllerContext = new ControllerContext{HttpContext = new DefaultHttpContext {User = new ClaimsPrincipal(identity)}};
    }

    //Code-Owner: Julia Sandner
    private Mentor SetupResponsibleMentor(int mentorID, Trainee trainee)
    {
        var mentor = new Mentor{Id = mentorID, AssignedTrainees = new List<Trainee>{trainee}};
        _mentorRepoMock.Setup(r => r.GetMentorById(mentorID)).Returns(mentor);
        return mentor;
    }

    //code-owner: Julia Sandner
    //unit-test for Mentor accepting an assignment for a trainee (successful)
    [Fact]
    public void Accept_ValidAssignment_UpdateStatusAndRedirectToOverview()
    {
        // Arrange
        var trainee = new Trainee{Id = 41};
        var assignment = new LessonAssignment{Id = 5, TraineeId = 41, Trainee = trainee};
        _assignmentRepoMock.Setup(r => r.GetById(5)).Returns(assignment);
        _assignmentServiceMock.Setup(s => s.UpdateAssignmentStatus(5, LessonAssignmentStatus.Accepted)).Returns(assignment);
        SetupResponsibleMentor(1, trainee);

        //Act
        var result = _controller.AcceptAssignment(5);

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
        var result = _controller.AcceptAssignment(999);

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
        var trainee = new Trainee{Id = 41};
        var assignment = new LessonAssignment{Id = 5, TraineeId = 41, Trainee = trainee, Status = LessonAssignmentStatus.Open};
        _assignmentRepoMock.Setup(r => r.GetById(5)).Returns(assignment);
        _assignmentServiceMock.Setup(s => s.UpdateAssignmentStatus(5, LessonAssignmentStatus.Accepted)).Throws(new InvalidOperationException("Invalid status transition for assignment 5: Open -> Accepted."));
        SetupResponsibleMentor(1, trainee);

        //Act
        var result = _controller.AcceptAssignment(5);

        //Assert
        var redirect =Assert.IsType<RedirectToActionResult>(result);
        Assert.Equal("AssignmentOverview", redirect.ActionName);
        Assert.Equal(41, redirect.RouteValues["traineeId"]);
        Assert.NotNull(_controller.TempData["Error"]);
    }

    //Code-Owner: Julia Sandner
    //unit-test for Mentor accepting an assignmnet without authorization
    [Fact]
    public void Accept_NoUserClaim_ReturnsUnauthorized()
    {   
        //Arrange
        var trainee = new Trainee{Id =41};
        var assignment = new LessonAssignment{ Id = 5, TraineeId = 41, Trainee = trainee};
        _assignmentRepoMock.Setup( t => t.GetById(5)).Returns(assignment);
        SetUser(null);

        //Act
        var result = _controller.AcceptAssignment(5);

        //Assert
        Assert.IsType<UnauthorizedResult>(result);
        _assignmentServiceMock.Verify(s => s.UpdateAssignmentStatus(It.IsAny<int>(), It.IsAny<LessonAssignmentStatus>()), Times.Never);

    }

    //Code-Owner: Julia Sandner
    //Unit-test for Mentor accepting an assignment but mentor is not assigned to trainee
    [Fact]
    public void Accept_MentorNotAssignedToTrainee_ReturnsForbid()
    {   
        //Arrange
        var trainee = new Trainee{Id =41};
        var otherTrainee = new Trainee{Id = 60};
        var assignment = new LessonAssignment{ Id = 5, TraineeId = 41, Trainee = trainee};
        _assignmentRepoMock.Setup( t => t.GetById(5)).Returns(assignment);
        SetupResponsibleMentor(1, otherTrainee);

        //Act
        var result = _controller.AcceptAssignment(5);

        //Assert
        Assert.IsType<ForbidResult>(result);
        _assignmentServiceMock.Verify(s => s.UpdateAssignmentStatus(It.IsAny<int>(), It.IsAny<LessonAssignmentStatus>()), Times.Never);
    }
    
    //Code-Owner: Julia Sandner
    //unit-test for Mentor skipping an assignment for a trainee (successful)
    [Fact]
    public void Skip_ValidAssignment_UpdateStatusAndRedirectToOverview()
    {
        //Arrange
        var trainee = new Trainee{Id = 42};
        var assignment = new LessonAssignment{Id = 7, TraineeId = 42, Trainee = trainee};
        _assignmentRepoMock.Setup(r => r.GetById(7)).Returns(assignment);
        _assignmentServiceMock.Setup( s=> s.UpdateAssignmentStatus(7, LessonAssignmentStatus.Skipped)).Returns(assignment);
        SetupResponsibleMentor(1, trainee);
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

    //Code-Owner: Julia Sandner
    //unit-test for Mentor skipping an assignmnet without authorization
    [Fact]
    public void Skip_NoUserClaim_ReturnsUnauthorized()
    {   
        //Arrange
        var trainee = new Trainee{Id =41};
        var assignment = new LessonAssignment{ Id = 5, TraineeId = 41, Trainee = trainee};
        _assignmentRepoMock.Setup( t => t.GetById(5)).Returns(assignment);
        SetUser(null);

        //Act
        var result = _controller.SkipAssignment(5);

        //Assert
        Assert.IsType<UnauthorizedResult>(result);
        _assignmentServiceMock.Verify(s => s.UpdateAssignmentStatus(It.IsAny<int>(), It.IsAny<LessonAssignmentStatus>()), Times.Never);

    }

    //Code-Owner: Julia Sandner
    //Unit-test for Mentor skipping an assignment but mentor is not assigned to trainee
    [Fact]
    public void Skip_MentorNotAssignedToTrainee_ReturnsForbid()
    {   
        //Arrange
        var trainee = new Trainee{Id =41};
        var otherTrainee = new Trainee{Id = 60};
        var assignment = new LessonAssignment{ Id = 5, TraineeId = 41, Trainee = trainee};
        _assignmentRepoMock.Setup( t => t.GetById(5)).Returns(assignment);
        SetupResponsibleMentor(1, otherTrainee);

        //Act
        var result = _controller.SkipAssignment(5);

        //Assert
        Assert.IsType<ForbidResult>(result);
        _assignmentServiceMock.Verify(s => s.UpdateAssignmentStatus(It.IsAny<int>(), It.IsAny<LessonAssignmentStatus>()), Times.Never);
    }

    //code-owner: Julia Sandner
    //unit-test for mentor reordering Assignments for a trainee
    [Fact]
    public void UpdateAssignmentPosition_CallServiceWithGivenOrderAndRedirect()
    {
        //Arrange
        var trainee = new Trainee{Id = 42};
        _traineeRepoMock.Setup( t => t.FindById(42)).Returns(trainee);
        SetupResponsibleMentor(1, trainee);
        var orderedIds = new List<int> {2, 1, 3};

        //Act
        var result = _controller.UpdateAssignmentOrder(trainee.Id, orderedIds);

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
        var trainee = new Trainee{Id = 42};
        _traineeRepoMock.Setup( t => t.FindById(42)).Returns(trainee);
        SetupResponsibleMentor(1, trainee);
        var orderedIds = new List<int>();

        //Act
        var result = _controller.UpdateAssignmentOrder(42, orderedIds);

        //Assert
        _assignmentServiceMock.Verify(r => r.UpdateAssignmentOrder(orderedIds), Times.Once);
        Assert.IsType<RedirectToActionResult>(result);
    }

    //Code-Owner: Julia Sandner
    //unit-test for Mentor reordering assignments without authorization
    [Fact]
    public void UpdateAssignmentPosition_NoUserClaim_ReturnsUnauthorized()
    {   
        //Arrange
        var trainee = new Trainee{Id =41};
        var assignment = new LessonAssignment{ Id = 5, TraineeId = 41, Trainee = trainee};
        _traineeRepoMock.Setup( t => t.FindById(41)).Returns(trainee);
        var orderedIds = new List<int> {2, 1, 3};
        SetUser(null);

        //Act
        var result = _controller.UpdateAssignmentOrder(41, orderedIds);

        //Assert
        Assert.IsType<UnauthorizedResult>(result);
        _assignmentServiceMock.Verify(r => r.UpdateAssignmentOrder(It.IsAny<List<int>>()), Times.Never);
    }

    //Code-Owner: Julia Sandner
    //Unit-test for Mentor reordering assignments but mentor is not assigned to trainee
    [Fact]
    public void UpdateAssignmentPosition_MentorNotAssignedToTrainee_ReturnsForbid()
    {   
        //Arrange
        var trainee = new Trainee{Id =41};
        var otherTrainee = new Trainee{Id = 60};
        var assignment = new LessonAssignment{ Id = 5, TraineeId = 41, Trainee = trainee};
        _traineeRepoMock.Setup( t => t.FindById(41)).Returns(trainee);
        SetupResponsibleMentor(1, otherTrainee);
        var orderedIds = new List<int> {2, 1, 3};

        //Act
        var result = _controller.UpdateAssignmentOrder(41, orderedIds);

        //Assert
        Assert.IsType<ForbidResult>(result);
         _assignmentServiceMock.Verify(r => r.UpdateAssignmentOrder(It.IsAny<List<int>>()), Times.Never);
    }

    //code-owner: Julia Sandner
    //unit-test for Mentor reordering assignments but trainee is unknown
    [Fact]
    public void UpdateAssignmentPosition_UnknownTrainee_ReturnsNotFound()
    {
        _traineeRepoMock.Setup(t => t.FindById(999)).Returns((Trainee?) null);
        var orderedIds = new List<int> {2, 1,3};

        //Act
        var result = _controller.UpdateAssignmentOrder(999, orderedIds);

        //Assert
        Assert.IsType<NotFoundResult>(result);
        _assignmentServiceMock.Verify( r => r.UpdateAssignmentOrder(It.IsAny<List<int>>()), Times.Never);
    }

    //Code-Owner: Julia Sandner
    //unit-test for mentor rejecting an assignmnet with a valid reason
    [Fact]
    public void Reject_ValidAssignmentAndReason_CallServiceAndRedirect()
    {
        //Arrange
        var trainee = new Trainee{Id = 44};
        var assignment = new LessonAssignment{Id = 8, TraineeId = 44, Trainee = trainee};
        _assignmentRepoMock.Setup(r => r.GetById(8)).Returns(assignment);
        SetupResponsibleMentor(1, trainee);

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
        var trainee = new Trainee{Id = 44};
        var assignment = new LessonAssignment{Id = 8, TraineeId = 44, Trainee = trainee};
        _assignmentRepoMock.Setup(r => r.GetById(8)).Returns(assignment);
        _assignmentServiceMock.Setup( s => s.RejectAssignment(8, "")).Throws(new ArgumentException("A Reason is required.", "reason"));
        SetupResponsibleMentor(1, trainee);

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
        var trainee = new Trainee{Id = 40};
        var assignment = new LessonAssignment{Id = 9, TraineeId = 40, Trainee = trainee, Status = LessonAssignmentStatus.Open};
        _assignmentRepoMock.Setup(r => r.GetById(9)).Returns(assignment);
        _assignmentServiceMock.Setup( s => s.RejectAssignment(9, "some reasons")).Throws(new InvalidOperationException("Invalid status transition for assignment 9: Open -> rejected"));
        SetupResponsibleMentor(1, trainee);

        //Act
        var result = _controller.RejectAssignment(9, "some reasons");

        //Assert
        var redirect = Assert.IsType<RedirectToActionResult>(result);
        Assert.Equal("AssignmentOverview", redirect.ActionName);
        Assert.Equal(40, redirect.RouteValues["traineeId"]);
        Assert.NotNull(_controller.TempData["Error"]);
    }

    //Code-Owner: Julia Sandner
    //unit-test for Mentor rejecting an assignmnet without authorization
    [Fact]
    public void Reject_NoUserClaim_ReturnsUnauthorized()
    {   
        //Arrange
        var trainee = new Trainee{Id =41};
        var assignment = new LessonAssignment{ Id = 5, TraineeId = 41, Trainee = trainee};
        _assignmentRepoMock.Setup( t => t.GetById(5)).Returns(assignment);
        SetUser(null);

        //Act
        var result = _controller.RejectAssignment(5, "some Reason");

        //Assert
        Assert.IsType<UnauthorizedResult>(result);
        _assignmentServiceMock.Verify(s => s.RejectAssignment(It.IsAny<int>(), It.IsAny<string>()), Times.Never);

    }

    //Code-Owner: Julia Sandner
    //Unit-test for Mentor rejecting an assignment but mentor is not assigned to trainee
    [Fact]
    public void Reject_MentorNotAssignedToTrainee_ReturnsForbid()
    {   
        //Arrange
        var trainee = new Trainee{Id =41};
        var otherTrainee = new Trainee{Id = 60};
        var assignment = new LessonAssignment{ Id = 5, TraineeId = 41, Trainee = trainee};
        _assignmentRepoMock.Setup( t => t.GetById(5)).Returns(assignment);
        SetupResponsibleMentor(1, otherTrainee);

        //Act
        var result = _controller.RejectAssignment(5, "some reason");

        //Assert
        Assert.IsType<ForbidResult>(result);
        _assignmentServiceMock.Verify(s => s.RejectAssignment(It.IsAny<int>(), It.IsAny<string>()), Times.Never);
    }

    //code-owner: Julia Sandner
    //unit-test for Mentor reopening an assignment for a trainee (successful)
    [Fact]
    public void OpenAssignment_ValidAssignment_UpdateStatusAndRedirectToOverview()
    {
        // Arrange
        var trainee = new Trainee{Id = 41};
        var assignment = new LessonAssignment{Id = 5, TraineeId = 41, Trainee = trainee};
        _assignmentRepoMock.Setup(r => r.GetById(5)).Returns(assignment);
        _assignmentServiceMock.Setup(s => s.UpdateAssignmentStatus(5, LessonAssignmentStatus.Open)).Returns(assignment);
        SetupResponsibleMentor(1, trainee);

        //Act
        var result = _controller.OpenAssignment(5);

        //Assert
        _assignmentServiceMock.Verify(r => r.UpdateAssignmentStatus(5, LessonAssignmentStatus.Open), Times.Once);
        var redirect = Assert.IsType<RedirectToActionResult>(result);
        Assert.Equal("AssignmentOverview", redirect.ActionName);
        Assert.Equal(41, redirect.RouteValues["traineeId"]);

    }

    //Code-Owner: Julia Sandner
    //unit-test for Mentor reopening an unknown assignment for a trainee 
    [Fact]
    public void OpenAssignment_UnknowenAssignment_ReturnNotFound()
    {
        //Arrange
        _assignmentRepoMock.Setup(r => r.GetById(It.IsAny<int>())).Returns((LessonAssignment?) null);

        //Act
        var result = _controller.OpenAssignment(999);

        //Assert
        Assert.IsType<NotFoundResult>(result);
        _assignmentServiceMock.Verify(s => s.UpdateAssignmentStatus(It.IsAny<int>(), It.IsAny<LessonAssignmentStatus>()), Times.Never);
    }

    //Code-Owner: Julia Sandner
    //unit-test for Mentor reopening an assignmnet that is in an invalid status for this transition
    [Fact]
    public void OpenAssignment_InvalidTransition_SetsErrorAndRedirectsToOverview()
    {
        //Arrange
        var trainee = new Trainee{Id = 41};
        var assignment = new LessonAssignment{Id = 5, TraineeId = 41, Trainee = trainee, Status = LessonAssignmentStatus.Rejected};
        _assignmentRepoMock.Setup(r => r.GetById(5)).Returns(assignment);
        _assignmentServiceMock.Setup(s => s.UpdateAssignmentStatus(5, LessonAssignmentStatus.Open)).Throws(new InvalidOperationException("Invalid status transition for assignment 5: Rejected -> Open."));
        SetupResponsibleMentor(1, trainee);

        //Act
        var result = _controller.OpenAssignment(5);

        //Assert
        var redirect =Assert.IsType<RedirectToActionResult>(result);
        Assert.Equal("AssignmentOverview", redirect.ActionName);
        Assert.Equal(41, redirect.RouteValues["traineeId"]);
        Assert.NotNull(_controller.TempData["Error"]);
    }

    //Code-Owner: Julia Sandner
    //unit-test for Mentor reopening an assignmnet without authorization
    [Fact]
    public void OpenAssignment_NoUserClaim_ReturnsUnauthorized()
    {   
        //Arrange
        var trainee = new Trainee{Id =41};
        var assignment = new LessonAssignment{ Id = 5, TraineeId = 41, Trainee = trainee};
        _assignmentRepoMock.Setup( t => t.GetById(5)).Returns(assignment);
        SetUser(null);

        //Act
        var result = _controller.AcceptAssignment(5);

        //Assert
        Assert.IsType<UnauthorizedResult>(result);
        _assignmentServiceMock.Verify(s => s.UpdateAssignmentStatus(It.IsAny<int>(), It.IsAny<LessonAssignmentStatus>()), Times.Never);

    }

    //Code-Owner: Julia Sandner
    //Unit-test for Mentor reopening an assignment but mentor is not assigned to trainee
    [Fact]
    public void OpenAssignment_MentorNotAssignedToTrainee_ReturnsForbid()
    {   
        //Arrange
        var trainee = new Trainee{Id =41};
        var otherTrainee = new Trainee{Id = 60};
        var assignment = new LessonAssignment{ Id = 5, TraineeId = 41, Trainee = trainee};
        _assignmentRepoMock.Setup( t => t.GetById(5)).Returns(assignment);
        SetupResponsibleMentor(1, otherTrainee);

        //Act
        var result = _controller.OpenAssignment(5);

        //Assert
        Assert.IsType<ForbidResult>(result);
        _assignmentServiceMock.Verify(s => s.UpdateAssignmentStatus(It.IsAny<int>(), It.IsAny<LessonAssignmentStatus>()), Times.Never);
    }

    //code-owner: Julia Sandner
    //unit-test for Mentor starting an assignment for a trainee (successful)
    [Fact]
    public void StartAssignment_ValidAssignment_UpdateStatusAndRedirectToOverview()
    {
        // Arrange
        var trainee = new Trainee{Id = 41};
        var assignment = new LessonAssignment{Id = 5, TraineeId = 41, Trainee = trainee};
        _assignmentRepoMock.Setup(r => r.GetById(5)).Returns(assignment);
        _assignmentServiceMock.Setup(s => s.UpdateAssignmentStatus(5, LessonAssignmentStatus.Started)).Returns(assignment);
        SetupResponsibleMentor(1, trainee);

        //Act
        var result = _controller.StartAssignment(5);

        //Assert
        _assignmentServiceMock.Verify(r => r.UpdateAssignmentStatus(5, LessonAssignmentStatus.Started), Times.Once);
        var redirect = Assert.IsType<RedirectToActionResult>(result);
        Assert.Equal("AssignmentOverview", redirect.ActionName);
        Assert.Equal(41, redirect.RouteValues["traineeId"]);

    }

    //Code-Owner: Julia Sandner
    //unit-test for Mentor starting an unknown assignment for a trainee 
    [Fact]
    public void StartAssignment_UnknowenAssignment_ReturnNotFound()
    {
        //Arrange
        _assignmentRepoMock.Setup(r => r.GetById(It.IsAny<int>())).Returns((LessonAssignment?) null);

        //Act
        var result = _controller.StartAssignment(999);

        //Assert
        Assert.IsType<NotFoundResult>(result);
        _assignmentServiceMock.Verify(s => s.UpdateAssignmentStatus(It.IsAny<int>(), It.IsAny<LessonAssignmentStatus>()), Times.Never);
    }

    //Code-Owner: Julia Sandner
    //unit-test for Mentor starting an assignmnet that is in an invalid status for this transition
    [Fact]
    public void StartAssignment_InvalidTransition_SetsErrorAndRedirectsToOverview()
    {
        //Arrange
        var trainee = new Trainee{Id = 41};
        var assignment = new LessonAssignment{Id = 5, TraineeId = 41, Trainee = trainee, Status = LessonAssignmentStatus.Rated};
        _assignmentRepoMock.Setup(r => r.GetById(5)).Returns(assignment);
        _assignmentServiceMock.Setup(s => s.UpdateAssignmentStatus(5, LessonAssignmentStatus.Started)).Throws(new InvalidOperationException("Invalid status transition for assignment 5: Rated -> Started."));
        SetupResponsibleMentor(1, trainee);

        //Act
        var result = _controller.StartAssignment(5);

        //Assert
        var redirect =Assert.IsType<RedirectToActionResult>(result);
        Assert.Equal("AssignmentOverview", redirect.ActionName);
        Assert.Equal(41, redirect.RouteValues["traineeId"]);
        Assert.NotNull(_controller.TempData["Error"]);
    }

    //Code-Owner: Julia Sandner
    //unit-test for Mentor starting an assignmnet without authorization
    [Fact]
    public void StartAssignment_NoUserClaim_ReturnsUnauthorized()
    {   
        //Arrange
        var trainee = new Trainee{Id =41};
        var assignment = new LessonAssignment{ Id = 5, TraineeId = 41, Trainee = trainee};
        _assignmentRepoMock.Setup( t => t.GetById(5)).Returns(assignment);
        SetUser(null);

        //Act
        var result = _controller.StartAssignment(5);

        //Assert
        Assert.IsType<UnauthorizedResult>(result);
        _assignmentServiceMock.Verify(s => s.UpdateAssignmentStatus(It.IsAny<int>(), It.IsAny<LessonAssignmentStatus>()), Times.Never);

    }

    //Code-Owner: Julia Sandner
    //Unit-test for Mentor starting an assignment but mentor is not assigned to trainee
    [Fact]
    public void StartAssignment_MentorNotAssignedToTrainee_ReturnsForbid()
    {   
        //Arrange
        var trainee = new Trainee{Id =41};
        var otherTrainee = new Trainee{Id = 60};
        var assignment = new LessonAssignment{ Id = 5, TraineeId = 41, Trainee = trainee};
        _assignmentRepoMock.Setup( t => t.GetById(5)).Returns(assignment);
        SetupResponsibleMentor(1, otherTrainee);

        //Act
        var result = _controller.StartAssignment(5);

        //Assert
        Assert.IsType<ForbidResult>(result);
        _assignmentServiceMock.Verify(s => s.UpdateAssignmentStatus(It.IsAny<int>(), It.IsAny<LessonAssignmentStatus>()), Times.Never);
    }

    //code-owner: Julia Sandner
    //unit-test for Mentor finishing an assignment for a trainee (successful)
    [Fact]
    public void FinishAssignment_ValidAssignment_UpdateStatusAndRedirectToOverview()
    {
        // Arrange
        var trainee = new Trainee{Id = 41};
        var assignment = new LessonAssignment{Id = 5, TraineeId = 41, Trainee = trainee};
        _assignmentRepoMock.Setup(r => r.GetById(5)).Returns(assignment);
        _assignmentServiceMock.Setup(s => s.UpdateAssignmentStatus(5, LessonAssignmentStatus.Finished)).Returns(assignment);
        SetupResponsibleMentor(1, trainee);

        //Act
        var result = _controller.FinishAssignment(5);

        //Assert
        _assignmentServiceMock.Verify(r => r.UpdateAssignmentStatus(5, LessonAssignmentStatus.Finished), Times.Once);
        var redirect = Assert.IsType<RedirectToActionResult>(result);
        Assert.Equal("AssignmentOverview", redirect.ActionName);
        Assert.Equal(41, redirect.RouteValues["traineeId"]);

    }

    //Code-Owner: Julia Sandner
    //unit-test for Mentor finishing an unknown assignment for a trainee 
    [Fact]
    public void FinishAssignment_UnknowenAssignment_ReturnNotFound()
    {
        //Arrange
        _assignmentRepoMock.Setup(r => r.GetById(It.IsAny<int>())).Returns((LessonAssignment?) null);

        //Act
        var result = _controller.FinishAssignment(999);

        //Assert
        Assert.IsType<NotFoundResult>(result);
        _assignmentServiceMock.Verify(s => s.UpdateAssignmentStatus(It.IsAny<int>(), It.IsAny<LessonAssignmentStatus>()), Times.Never);
    }

    //Code-Owner: Julia Sandner
    //unit-test for Mentor finishing an assignmnet that is in an invalid status for this transition
    [Fact]
    public void FinishAssignment_InvalidTransition_SetsErrorAndRedirectsToOverview()
    {
        //Arrange
        var trainee = new Trainee{Id = 41};
        var assignment = new LessonAssignment{Id = 5, TraineeId = 41, Trainee = trainee, Status = LessonAssignmentStatus.Open};
        _assignmentRepoMock.Setup(r => r.GetById(5)).Returns(assignment);
        _assignmentServiceMock.Setup(s => s.UpdateAssignmentStatus(5, LessonAssignmentStatus.Finished)).Throws(new InvalidOperationException("Invalid status transition for assignment 5: Open -> Finished."));
        SetupResponsibleMentor(1, trainee);

        //Act
        var result = _controller.FinishAssignment(5);

        //Assert
        var redirect =Assert.IsType<RedirectToActionResult>(result);
        Assert.Equal("AssignmentOverview", redirect.ActionName);
        Assert.Equal(41, redirect.RouteValues["traineeId"]);
        Assert.NotNull(_controller.TempData["Error"]);
    }

    //Code-Owner: Julia Sandner
    //unit-test for Mentor finishing an assignmnet without authorization
    [Fact]
    public void FinishAssignment_NoUserClaim_ReturnsUnauthorized()
    {   
        //Arrange
        var trainee = new Trainee{Id =41};
        var assignment = new LessonAssignment{ Id = 5, TraineeId = 41, Trainee = trainee};
        _assignmentRepoMock.Setup( t => t.GetById(5)).Returns(assignment);
        SetUser(null);

        //Act
        var result = _controller.FinishAssignment(5);

        //Assert
        Assert.IsType<UnauthorizedResult>(result);
        _assignmentServiceMock.Verify(s => s.UpdateAssignmentStatus(It.IsAny<int>(), It.IsAny<LessonAssignmentStatus>()), Times.Never);

    }

    //Code-Owner: Julia Sandner
    //Unit-test for Mentor finishing an assignment but mentor is not assigned to trainee
    [Fact]
    public void FinishAssignment_MentorNotAssignedToTrainee_ReturnsForbid()
    {   
        //Arrange
        var trainee = new Trainee{Id =41};
        var otherTrainee = new Trainee{Id = 60};
        var assignment = new LessonAssignment{ Id = 5, TraineeId = 41, Trainee = trainee};
        _assignmentRepoMock.Setup( t => t.GetById(5)).Returns(assignment);
        SetupResponsibleMentor(1, otherTrainee);

        //Act
        var result = _controller.FinishAssignment(5);

        //Assert
        Assert.IsType<ForbidResult>(result);
        _assignmentServiceMock.Verify(s => s.UpdateAssignmentStatus(It.IsAny<int>(), It.IsAny<LessonAssignmentStatus>()), Times.Never);
    }

    //code-owner: Julia Sandner
    //unit-test for Mentor changing an assignment's state to rated without giving Feedback for a trainee (successful)
    [Fact]
    public void RateAssignmentWithoutFeedback_ValidAssignment_UpdateStatusAndRedirectToOverview()
    {
        // Arrange
        var trainee = new Trainee{Id = 41};
        var assignment = new LessonAssignment{Id = 5, TraineeId = 41, Trainee = trainee};
        _assignmentRepoMock.Setup(r => r.GetById(5)).Returns(assignment);
        _assignmentServiceMock.Setup(s => s.UpdateAssignmentStatus(5, LessonAssignmentStatus.Rated)).Returns(assignment);
        SetupResponsibleMentor(1, trainee);

        //Act
        var result = _controller.RateAssignmentWithoutFeedback(5);

        //Assert
        _assignmentServiceMock.Verify(r => r.UpdateAssignmentStatus(5, LessonAssignmentStatus.Rated), Times.Once);
        var redirect = Assert.IsType<RedirectToActionResult>(result);
        Assert.Equal("AssignmentOverview", redirect.ActionName);
        Assert.Equal(41, redirect.RouteValues["traineeId"]);

    }

    //Code-Owner: Julia Sandner
    //unit-test for Mentor changing an unkown assignment's state to rated for a trainee 
    [Fact]
    public void RateAssignmentWithoutFeedback_UnknowenAssignment_ReturnNotFound()
    {
        //Arrange
        _assignmentRepoMock.Setup(r => r.GetById(It.IsAny<int>())).Returns((LessonAssignment?) null);

        //Act
        var result = _controller.RateAssignmentWithoutFeedback(999);

        //Assert
        Assert.IsType<NotFoundResult>(result);
        _assignmentServiceMock.Verify(s => s.UpdateAssignmentStatus(It.IsAny<int>(), It.IsAny<LessonAssignmentStatus>()), Times.Never);
    }

    //Code-Owner: Julia Sandner
    //unit-test for Mentor rating (without Feedback) an assignmnet that is in an invalid status for this transition
    [Fact]
    public void RateAssignmentWithoutFeedback_InvalidTransition_SetsErrorAndRedirectsToOverview()
    {
        //Arrange
        var trainee = new Trainee{Id = 41};
        var assignment = new LessonAssignment{Id = 5, TraineeId = 41, Trainee = trainee, Status = LessonAssignmentStatus.Open};
        _assignmentRepoMock.Setup(r => r.GetById(5)).Returns(assignment);
        _assignmentServiceMock.Setup(s => s.UpdateAssignmentStatus(5, LessonAssignmentStatus.Rated)).Throws(new InvalidOperationException("Invalid status transition for assignment 5: Open -> Rated."));
        SetupResponsibleMentor(1, trainee);

        //Act
        var result = _controller.RateAssignmentWithoutFeedback(5);

        //Assert
        var redirect =Assert.IsType<RedirectToActionResult>(result);
        Assert.Equal("AssignmentOverview", redirect.ActionName);
        Assert.Equal(41, redirect.RouteValues["traineeId"]);
        Assert.NotNull(_controller.TempData["Error"]);
    }

    //Code-Owner: Julia Sandner
    //unit-test for Mentor rating (without Feedback) an assignmnet without authorization
    [Fact]
    public void RateAssignmentWithoutFeedback_NoUserClaim_ReturnsUnauthorized()
    {   
        //Arrange
        var trainee = new Trainee{Id =41};
        var assignment = new LessonAssignment{ Id = 5, TraineeId = 41, Trainee = trainee};
        _assignmentRepoMock.Setup( t => t.GetById(5)).Returns(assignment);
        SetUser(null);

        //Act
        var result = _controller.RateAssignmentWithoutFeedback(5);

        //Assert
        Assert.IsType<UnauthorizedResult>(result);
        _assignmentServiceMock.Verify(s => s.UpdateAssignmentStatus(It.IsAny<int>(), It.IsAny<LessonAssignmentStatus>()), Times.Never);

    }

    //Code-Owner: Julia Sandner
    //Unit-test for Mentor rating (without Feedback) an assignment but mentor is not assigned to trainee
    [Fact]
    public void RateAssignmentWithoutFeedback_MentorNotAssignedToTrainee_ReturnsForbid()
    {   
        //Arrange
        var trainee = new Trainee{Id =41};
        var otherTrainee = new Trainee{Id = 60};
        var assignment = new LessonAssignment{ Id = 5, TraineeId = 41, Trainee = trainee};
        _assignmentRepoMock.Setup( t => t.GetById(5)).Returns(assignment);
        SetupResponsibleMentor(1, otherTrainee);

        //Act
        var result = _controller.RateAssignmentWithoutFeedback(5);

        //Assert
        Assert.IsType<ForbidResult>(result);
        _assignmentServiceMock.Verify(s => s.UpdateAssignmentStatus(It.IsAny<int>(), It.IsAny<LessonAssignmentStatus>()), Times.Never);
    }

}