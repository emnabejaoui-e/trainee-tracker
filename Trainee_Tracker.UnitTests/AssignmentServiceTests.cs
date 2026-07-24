using System.Runtime.CompilerServices;
using Moq;
using Trainee_Tracker.Data.LessonAssignments;
using Trainee_Tracker.Data.Lessons;
using Trainee_Tracker.Data.Rejections;
using Trainee_Tracker.Models;
using Trainee_Tracker.Services;

namespace Trainee_Tracker.UnitTests;

public class AssignmentServiceTests
{
    private readonly Mock<ILessonAssignmentRepository> _assignmentRepoMock;
    private readonly Mock<ILessonRepository> _lessonRepoMock;
    private readonly Mock<IRejectionRepository> _rejectionRepoMock;
    private readonly AssignmentService _service;

    public AssignmentServiceTests()
    {   
        _lessonRepoMock = new Mock<ILessonRepository>();
        _rejectionRepoMock = new Mock<IRejectionRepository>();
        _assignmentRepoMock = new Mock<ILessonAssignmentRepository>();

        _service = new AssignmentService(_lessonRepoMock.Object, _assignmentRepoMock.Object, _rejectionRepoMock.Object, null);

    }

    //Code-Owner: Julia Sandner
    /// <summary>
    /// untitest for starting an assignment in state open (sucessful)
    /// </summary>
    [Fact]
    public void UpdateAssignmentStatus_startFromOpen_SetsStartedAndReturnAssignment()
    {   
        //Arrange
        var assignment = new LessonAssignment { Id = 5, Status = LessonAssignmentStatus.Open};
        _assignmentRepoMock.Setup(r => r.GetById(5)).Returns(assignment);

        //Act
        var result = _service.UpdateAssignmentStatus(5, LessonAssignmentStatus.Started);

        //Assert
        _assignmentRepoMock.Verify(r => r.UpdateStatus(5, LessonAssignmentStatus.Started), Times.Once);
        Assert.Equal(assignment, result);
    }

    //Code owner: Julia Sandner
    /// <summary>
    /// unittest for starting a assignmnet in an invalid state (throws invalidOperation-Exception)
    /// </summary>
    [Fact]
    public void UpdateAssignmentStatus_StartFromInvalidStatus_ThrowsInvalidOperationException()
    {
        //arrange
        var assignment = new LessonAssignment{Id = 5, Status = LessonAssignmentStatus.Rated};
        _assignmentRepoMock.Setup(s => s.GetById(5)).Returns(assignment);

        Assert.Throws<InvalidOperationException>(() => _service.UpdateAssignmentStatus(5, LessonAssignmentStatus.Started));
        _assignmentRepoMock.Verify(r => r.UpdateStatus(It.IsAny<int>(), It.IsAny<LessonAssignmentStatus>()), Times.Never);

    }

    //code-Owner: julia Sandner
    /// <summary>
    /// unittest for finishing a started assignment (successful)
    /// </summary>
    [Fact]
    public void UpdateAssignmentStatus_FinishFromStarted_SetsFinishAndReturnAssignment()
    {   
        //Arrange
        var assignment = new LessonAssignment { Id = 5, Status = LessonAssignmentStatus.Started};
        _assignmentRepoMock.Setup(r => r.GetById(5)).Returns(assignment);

        //Act
        var result = _service.UpdateAssignmentStatus(5, LessonAssignmentStatus.Finished);

        //Assert
        _assignmentRepoMock.Verify(r => r.UpdateStatus(5, LessonAssignmentStatus.Finished), Times.Once);
        Assert.Equal(assignment, result);
    }


    //Code-Owner: Julia Sandner
    /// <summary>
    /// unittest for finishing an  assignment in an invalid state (throws exception)
    /// </summary>
    [Fact]
    public void UpdateAssignmentStatus_FinishFromInvalidStatus_ThrowsInvalidOperationException()
    {
        //arrange
        var assignment = new LessonAssignment{Id = 5, Status = LessonAssignmentStatus.Rated};
        _assignmentRepoMock.Setup(s => s.GetById(5)).Returns(assignment);

        Assert.Throws<InvalidOperationException>(() => _service.UpdateAssignmentStatus(5, LessonAssignmentStatus.Finished));
        _assignmentRepoMock.Verify(r => r.UpdateStatus(It.IsAny<int>(), It.IsAny<LessonAssignmentStatus>()), Times.Never);

    }
    //Code-Owner: Julia Sandner
    /// <summary>
    /// test for accepting a finished assignment (sucessful)
    /// </summary>
    [Fact]
    public void UpdateAssignmentStatus_AcceptFromFinished_SetsFinishAndReturnAssignment()
    {   
        //Arrange
        var assignment = new LessonAssignment { Id = 5, Status = LessonAssignmentStatus.Finished};
        _assignmentRepoMock.Setup(r => r.GetById(5)).Returns(assignment);

        //Act
        var result = _service.UpdateAssignmentStatus(5, LessonAssignmentStatus.Accepted);

        //Assert
        _assignmentRepoMock.Verify(r => r.UpdateStatus(5, LessonAssignmentStatus.Accepted), Times.Once);
        Assert.Equal(assignment, result);
    }

    //Code-Owner: Julia Sandner
    /// <summary>
    /// test for accepting an assignment in an invalid state (throws exception)
    /// </summary>
    [Fact]
    public void UpdateAssignmentStatus_AcceptFromInvalidStatus_ThrowsInvalidOperationException()
    {
        //arrange
        var assignment = new LessonAssignment{Id = 5, Status = LessonAssignmentStatus.Started};
        _assignmentRepoMock.Setup(s => s.GetById(5)).Returns(assignment);

        Assert.Throws<InvalidOperationException>(() => _service.UpdateAssignmentStatus(5, LessonAssignmentStatus.Accepted));
        _assignmentRepoMock.Verify(r => r.UpdateStatus(It.IsAny<int>(), It.IsAny<LessonAssignmentStatus>()), Times.Never);

    }

    //Code-Owner: Julia Sandner
    /// <summary>
    /// test for skipping an open assignment (successful)
    /// </summary>
    [Fact]
    public void UpdateAssignmentStatus_SkipFromOpen_SetsFinishAndReturnAssignment()
    {   
        //Arrange
        var assignment = new LessonAssignment { Id = 5, Status = LessonAssignmentStatus.Open};
        _assignmentRepoMock.Setup(r => r.GetById(5)).Returns(assignment);

        //Act
        var result = _service.UpdateAssignmentStatus(5, LessonAssignmentStatus.Skipped);

        //Assert
        _assignmentRepoMock.Verify(r => r.UpdateStatus(5, LessonAssignmentStatus.Skipped), Times.Once);
        Assert.Equal(assignment, result);
    }

    //Code-Owner: Julia Sandner
    /// <summary>
    /// test for skipping an assignment in an invalid state (throws exception)
    /// </summary>
    [Fact]
    public void UpdateAssignmentStatus_SkipFromInvalidStatus_ThrowsInvalidOperationException()
    {
        //arrange
        var assignment = new LessonAssignment{Id = 5, Status = LessonAssignmentStatus.Accepted};
        _assignmentRepoMock.Setup(s => s.GetById(5)).Returns(assignment);

        Assert.Throws<InvalidOperationException>(() => _service.UpdateAssignmentStatus(5, LessonAssignmentStatus.Skipped));
        _assignmentRepoMock.Verify(r => r.UpdateStatus(It.IsAny<int>(), It.IsAny<LessonAssignmentStatus>()), Times.Never);

    }

    //Code-Owner: Julia Sandner
    /// <summary>
    /// test for rejecting an finished assignment (successful)
    /// </summary>
    [Fact]
    public void RejectAssignment_ValidFinishedStatusAndReason_SetRejectedAndReturnRejection()
    {
        var assignment = new LessonAssignment{Id = 5, Status = LessonAssignmentStatus.Finished};
        var reason = "some Reasons";
        var rejection = new Rejection {AssignmentId = 5, Reason = reason};
        _assignmentRepoMock.Setup( r=> r.GetById(5)).Returns(assignment);
        _rejectionRepoMock.Setup(r => r.Reject(5, reason)).Returns(rejection);

        //Act
        var result = _service.RejectAssignment(5, reason);

        //Assert
        _rejectionRepoMock.Verify( s => s.Reject(5, reason), Times.Once);
        Assert.Equal( rejection, result);
    }

    //code-owner: Julia Sandner
    /// <summary>
    /// test for rejecting an assignment with empty reason (throws exception)
    /// </summary>
    [Fact]
    public void RejectAssignment_EmptyReason_ThrowsArgumentException()
    {
        var assignment = new LessonAssignment{ Id = 5, Status = LessonAssignmentStatus.Finished};
        _assignmentRepoMock.Setup( r=> r.GetById(5)).Returns(assignment);

        Assert.Throws<ArgumentException>(() => _service.RejectAssignment(5, ""));

        _rejectionRepoMock.Verify( r => r.Reject(It.IsAny<int>(), It.IsAny<string>()), Times.Never);

    }

    //Code-Owner: Julia Sandner
    /// <summary>
    /// test for rejecting an assignment in a invalid state (throws exception)
    /// </summary>
    [Fact]
    public void RejectAssignment_InvalidStatus_ThrowsInvalidOperationException()
    {
        var assignment = new LessonAssignment{Id = 5, Status = LessonAssignmentStatus.Started};
        _assignmentRepoMock.Setup( r=> r.GetById(5)).Returns(assignment);

        Assert.Throws<InvalidOperationException>(() => _service.RejectAssignment(5, "some Reasons"));

        _rejectionRepoMock.Verify( r=> r.Reject(It.IsAny<int>(), It.IsAny<string>()), Times.Never);
    }

    //Code-Owner: Julia Sandner
    /// <summary>
    /// test for rejecting an unknown assignment (throws exception)
    /// </summary>
    [Fact]
    public void RejectAssignment_UnknownAssignment_ReturnsNull()
    {
        _assignmentRepoMock.Setup(r => r.GetById(It.IsAny<int>())).Returns((LessonAssignment?) null);
        
        var result = _service.RejectAssignment(5, "some Reasons");

        Assert.Null(result);
        _rejectionRepoMock.Verify( r => r.Reject(It.IsAny<int>(), It.IsAny<string>()), Times.Never);
    }

    //code-owner: Julia Sandner
    /// <summary>
    /// test for reopening a started assignment (successful)
    /// </summary>
    [Fact]
    public void UpdateAssignmentStatus_OpenFromStarted_SetOpenAndReturnAssignment()
    {
        //arrange
        var assignment = new LessonAssignment {Id = 5, Status = LessonAssignmentStatus.Started};
        _assignmentRepoMock.Setup(d => d.GetById(5)).Returns(assignment);

        //act
        var result = _service.UpdateAssignmentStatus(5, LessonAssignmentStatus.Open);

        //Assert
        _assignmentRepoMock.Verify( r => r.UpdateStatus(5, LessonAssignmentStatus.Open), Times.Once);
        Assert.Equal(assignment, result);
    }

    //code-owner: Julia Sandner
    /// <summary>
    /// test for restarting a finished assignment (successful)
    /// </summary>
    [Fact]
    public void UpdateAssignmentStatus_StartFromFinished_SetOpenAndReturnAssignment()
    {
        //arrange
        var assignment = new LessonAssignment {Id = 5, Status = LessonAssignmentStatus.Finished};
        _assignmentRepoMock.Setup(d => d.GetById(5)).Returns(assignment);

        //act
        var result = _service.UpdateAssignmentStatus(5, LessonAssignmentStatus.Started);

        //Assert
        _assignmentRepoMock.Verify( r => r.UpdateStatus(5, LessonAssignmentStatus.Started), Times.Once);
        Assert.Equal(assignment, result);
    }

    //code-owner: Julia Sandner
    /// <summary>
    /// test for restarting a rejected assignment (successful)
    /// </summary>
    [Fact]
    public void UpdateAssignmentStatus_StartFromRejected_SetOpenAndReturnAssignment()
    {
        //arrange
        var assignment = new LessonAssignment {Id = 5, Status = LessonAssignmentStatus.Rejected};
        _assignmentRepoMock.Setup(d => d.GetById(5)).Returns(assignment);

        //act
        var result = _service.UpdateAssignmentStatus(5, LessonAssignmentStatus.Started);

        //Assert
        _assignmentRepoMock.Verify( r => r.UpdateStatus(5, LessonAssignmentStatus.Started), Times.Once);
        Assert.Equal(assignment, result);
    }

    //code-owner: Julia Sandner
    /// <summary>
    /// test for rating an accepted assignment (successful)
    /// </summary>
    [Fact]
    public void UpdateAssignmentStatus_RateFromAccepted_SetOpenAndReturnAssignment()
    {
        //arrange
        var assignment = new LessonAssignment {Id = 5, Status = LessonAssignmentStatus.Accepted};
        _assignmentRepoMock.Setup(d => d.GetById(5)).Returns(assignment);

        //act
        var result = _service.UpdateAssignmentStatus(5, LessonAssignmentStatus.Rated);

        //Assert
        _assignmentRepoMock.Verify( r => r.UpdateStatus(5, LessonAssignmentStatus.Rated), Times.Once);
        Assert.Equal(assignment, result);
    }

    //code-owner: Julia Sandner
    /// <summary>
    /// test for refinishing an accepted assignment (successful)
    /// </summary>
    [Fact]
    public void UpdateAssignmentStatus_FinishFromAccepted_SetOpenAndReturnAssignment()
    {
        //arrange
        var assignment = new LessonAssignment {Id = 5, Status = LessonAssignmentStatus.Accepted};
        _assignmentRepoMock.Setup(d => d.GetById(5)).Returns(assignment);

        //act
        var result = _service.UpdateAssignmentStatus(5, LessonAssignmentStatus.Finished);

        //Assert
        _assignmentRepoMock.Verify( r => r.UpdateStatus(5, LessonAssignmentStatus.Finished), Times.Once);
        Assert.Equal(assignment, result);
    }

    //code-owner: Julia Sandner
    /// <summary>
    /// test for reopening a skipped assignment (successful)
    /// </summary>
    [Fact]
    public void UpdateAssignmentStatus_OpenFromSkipped_SetOpenAndReturnAssignment()
    {
        //arrange
        var assignment = new LessonAssignment {Id = 5, Status = LessonAssignmentStatus.Skipped};
        _assignmentRepoMock.Setup(d => d.GetById(5)).Returns(assignment);

        //act
        var result = _service.UpdateAssignmentStatus(5, LessonAssignmentStatus.Open);

        //Assert
        _assignmentRepoMock.Verify( r => r.UpdateStatus(5, LessonAssignmentStatus.Open), Times.Once);
        Assert.Equal(assignment, result);
    }

}