using System;
using System.IO;
using Notes.Models;
using Xamarin.Forms;

namespace Notes.Views
{
    [QueryProperty(nameof(ItemId), nameof(ItemId))]
    public partial class NoteEntryPage : ContentPage
    {
        public string ItemId
        {
            set
            {
                LoadNote(value);
            }
        }

        public NoteEntryPage()
        {
            InitializeComponent();

            // Set the BindingContext of the page to a new Note.
            BindingContext = new Note();
        }

        async void LoadNote(string itemId)
        {
            try
            {
                int id = Convert.ToInt32(itemId);
                // Retrieve the note and set it as the BindingContext of the page.
                Note note = await App.Database.GetNoteAsync(id);
                BindingContext = note;
            }
            catch (Exception)
            {
                Console.WriteLine("Failed to load note.");
            }
        }

        async void OnSaveButtonClicked(object sender, EventArgs e)
        {
            //Aquí es donde se conecta al la otra capa usando la métologia MVVM para enviarlo a los datos**
            var note = (Note)BindingContext;

            note.Date = DateTime.UtcNow;
            if (!string.IsNullOrWhiteSpace(note.Text) && !string.IsNullOrWhiteSpace(note.Name) && !string.IsNullOrWhiteSpace(note.Surname))
            {
                await App.Database.SaveNoteAsync(note);

                // Navigate backwards
                //await Shell.Current.Navigation();
                //await App.Current.MainPage = AppShell();
                await Shell.Current.GoToAsync("..");

            }
            else
            {
                await App.Current.MainPage.DisplayAlert("Aviso", "Por favor ingrese todos los datos", "OK");
            }

        }

        async void OnDeleteButtonClicked(object sender, EventArgs e)
        {
            var note = (Note)BindingContext;
            await App.Database.DeleteNoteAsync(note);

            // Navigate backwards
            await Shell.Current.GoToAsync("..");
        }
    }
}