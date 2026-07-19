using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Trainee_Tracker.Models;

namespace Trainee_Tracker.Services;

// Code-Owner: Andrej Basara
public interface IAssignmentService
{
    public void AssignLessonsToTrainee(Trainee trainee);

    //code-owner: Julia Sandner
    public AssignmentOverviewResult GetOverview(Trainee trainee);

    //Code-Owner: Julia Sandner
    public LessonAssignment? UpdateAssignmentStatus(int assignmentId, LessonAssignmentStatus newStatus);

    //Code-Owner: Julia Sandner
    public void UpdateAssignmentOrder(List<int> orderedIds);

    //Code-Owner: Julia Sandner
    public Rejection? RejectAssignment(int assignmentId, string reason);

    //Code-Owner: Julia Sandner
    public LessonAssignment? GetAcceptedAssignmentWithoutFeedback(Trainee trainee);

    //Code-Owner: Julia Sandner
    public void MarkFeedbackReminerAsShown(LessonAssignment assignment);


}