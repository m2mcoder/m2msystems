using System;
using System.Configuration;
using System.Linq;
using System.Transactions;
using M2MSystems.Access.Linq2Sql;
using M2MSystems.DataAccess.Entities;

namespace M2MSystems.DataAccess.DataAccessLayer
{
    public interface IApplicantsUpdater
    {
        bool Update(Application application);
    }

    public class ApplicantsUpdater : IApplicantsUpdater
    {
        public bool Update(Application application)
        {
            var connectionString = ConfigurationManager.
                ConnectionStrings["M2MSystemsConnectionString"].ConnectionString;

            var db = new M2MSystemsDataContext(connectionString);

            using (var transaction = new TransactionScope())
            {
                try
                {
                    var applicant = db.Applicants.FirstOrDefault(x => x.email == application.Email);
                    if (applicant != null)
                    {
                        applicant.address1 = application.Address1;
                        applicant.address2 = application.Address2;
                        applicant.city = application.City;
                        applicant.firstname = application.FirstName;
                        applicant.lastname = application.LastName;
                        application.ThirdPartyKey = application.ThirdPartyKey;
                    }
                    else
                    {
                        applicant = new Applicant
                        {
                            address1 = application.Address1,
                            address2 = application.Address2,
                            city = application.City,
                            firstname = application.FirstName,
                            lastname = application.LastName,
                            thirdpartykey = application.ThirdPartyKey,
                            email = application.Email
                        };
                        db.Applicants.InsertOnSubmit(applicant);
                    }
                    //var policy = new Policy
                    //{
                    //    cost = fee,
                    //    Product = form.Product,
                    //    enddate = application.EndDate,
                    //    startdate = application.StartDate
                    //};
                    //db.Policies.InsertOnSubmit(policy);
                    //var savedForm = new SavedForm
                    //{
                    //    answers = answersJson,
                    //    Applicant = applicant,
                    //    Form = form,
                    //    Policy = policy,
                    //};
                    //applicant.SavedForms.Add(savedForm);
                    //policy.SavedForms.Add(savedForm);

                    db.SubmitChanges();
                    transaction.Complete();
                }
                catch (Exception e)
                {
                    return false;
                }
                return true;
            }
        }
    }
}