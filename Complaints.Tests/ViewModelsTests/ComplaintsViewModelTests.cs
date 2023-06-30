using Complaints_Tests.Services;
using Complaints_WPF.Models;
using Complaints_WPF.Services.Interfaces;
using Complaints_WPF.ViewModels;
using Moq;

namespace Complaints_Tests.ViewModelsTests
{
    public class ComplaintsViewModelTests
    {
        [Fact]
        public void LoadData_ShouldReturn_ExpecedComplaints()
        {
            var testYear = "2020";
            var expectedComplainsList = GetComplainsTestList(5);

            var complaintServiceMock = new Mock<IComplaintService>();
            complaintServiceMock.Setup(s => s.GetAllComplaintsByYear(testYear)).Returns(expectedComplainsList);
            complaintServiceMock.Setup(s => s.LoadOZhClassification()).Returns(GetRandom.StringCollection(3, 10).ToList());
            complaintServiceMock.Setup(s => s.LoadResults()).Returns(GetRandom.StringCollection(3, 10).ToList());
            complaintServiceMock.Setup(s => s.LoadProsecutors()).Returns(GetRandom.StringCollection(3, 10).ToList());
            complaintServiceMock.Setup(s => s.LoadChiefs()).Returns(GetRandom.StringCollection(3, 10).ToList());
            complaintServiceMock.Setup(s => s.LoadCategories()).Returns(GetRandom.StringCollection(3, 10).ToList());

            var viewModel = new ComplaintsViewModel(complaintServiceMock.Object);

            Assert.Equal(5, viewModel.ComplaintsList.Count());
        }


        private IEnumerable<Complaint> GetComplainsTestList(int capacity)
        {
            var complaintsList = new Complaint[capacity];

            for (int i = 0; i < capacity; i++)
            {
                complaintsList[i] = new Complaint()
                {
                    ComplaintID = GetRandom.Id(),
                    ReceiptDate = GetRandom.DateTime(),

                    Citizen = GetRandomCitizen(),
                    OZhComplaintText = GetRandomOZhComplaintText(),
                    Prosecutor = GetRandomProsecutor(),
                    Result = GetRandomResult(),
                    Chief = GetRandomChief()
                };
            }

            return complaintsList;
        }

        private Chief GetRandomChief()
        {
            return new Chief()
            {
                ChiefID = GetRandom.Id(),
                ChiefName = GetRandom.String()
            };
        }

        private Result GetRandomResult()
        {
            return new Result()
            {
                ResultID = GetRandom.Id(),
                Rezolution = GetRandom.String()
            };
        }

        private Prosecutor GetRandomProsecutor()
        {
            return new Prosecutor
            {
                ProsecutorID = GetRandom.Id(),
                ProsecutorName = GetRandom.String()
            };
        }

        private OZhClassification GetRandomOZhComplaintText()
        {
            return new OZhClassification()
            {
                OZhComplaint = GetRandom.String(),
            };
        }

        private Citizen GetRandomCitizen()
        {
            return new Citizen()
            {
                CitizenID = GetRandom.Id(),
                CitizenName = GetRandom.String()
            };
        }

    }
}
