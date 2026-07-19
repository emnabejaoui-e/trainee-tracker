using Trainee_Tracker.Models;

namespace Trainee_Tracker.Data.Rejections;

// Code-Owner: Julia Sandner
public interface IRejectionRepository
{
    public IEnumerable<Rejection> GetRejectedByTrainee(Trainee trainee);
    public Rejection? GetById(int id);
    public Rejection Reject(int assignmentId, string reason);
    public IEnumerable<Rejection> GetRejectionsByAssignmentId(int assignmentId);
    public IDictionary<int, List<Rejection>> GetByAssignmentIds(IEnumerable<int> assignmentIds);
    public void Add(Rejection rejection);

}