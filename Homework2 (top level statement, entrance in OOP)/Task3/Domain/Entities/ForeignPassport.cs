using Task3.Domain.ValueObjects;

namespace Task3.Domain.Entities
{
    internal class ForeignPassport
    {
        public Guid Id { get; }
        public FullName FullName { get; set; }

        private DateTime _issueTime;
        public DateTime IssueDate
        {
            get => this._issueTime;
            set
            {
                if (value > DateTime.Now)
                    throw new ArgumentOutOfRangeException("Date of issue is incorrect");

                this._issueTime = value;
            }
        }

        public ForeignPassport(Guid id, FullName fullName, DateTime issueDate)
        {
            this.Id = id;
            this.FullName = fullName;
            this.IssueDate = issueDate;
        }

        public override string ToString() => $"Id: {this.Id}\nFull name: {this.FullName}\nIssue date: {this.IssueDate}";
    }
}
